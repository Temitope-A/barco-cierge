using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cierge.Data;
using Cierge.Models.AdminViewModels;
using Cierge.Models.ManageViewModels;
using Cierge.Services;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Cierge.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class AdminController : Controller
    {
        private readonly EventsService _events;
        private readonly NoticeService _notice;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AdminController(
            EventsService events,
            NoticeService notice,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _events = events;
            _notice = notice;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View(new AdminViewModel()
            {
                Users = _context.Users.Take(10).OrderBy(u => u.DateCreated).ToList(),
                UserCount = _userManager.Users.Count()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AdminViewModel model)
        {
            var searchTerm = _userManager.NormalizeKey(model.SearchTerm);

            model.Users = _context.Users.Where(u => u.NormalizedUserName.Contains(searchTerm) ||
                                                   u.NormalizedEmail.Contains(searchTerm) ||
                                                   u.FullName.Contains(searchTerm) ||
                                                   u.Id.Contains(searchTerm))
                                        .ToList();
            model.UserCount = model.Users.Count();

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Lockout(string userName, int minutes = 120)
        {
            var user = await _userManager.FindByNameAsync(userName);

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.UpdateSecurityStampAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(minutes)));

            return _notice.Success(this, $"User {user.UserName} locked out for {minutes} minutes.");
        }

        [HttpGet]
        public async Task<IActionResult> Impersonate(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);

            return _notice.Success(this, $"You are now logged-in as {user.UserName}.", "Don't forget to log out later.");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Email,Name,PinCode")] BarcoMemberViewModel barcoMember)
        {
            //Register locally
            var email = _userManager.NormalizeKey(barcoMember.Email);

            var userEmpty = new ApplicationUser()
            {
                UserName = email,
                Email = email,
                DateCreated = DateTimeOffset.UtcNow,
                SecurityStamp = new Guid().ToString(),

                FullName = barcoMember.Name,
                NickName = barcoMember.Name.Split()[0],
                PinCode = barcoMember.PinCode,
                EmailConfirmed = true
            };

            var userWithConfirmedEmail = await _userManager.FindByLoginAsync("Email", email);

            if (userWithConfirmedEmail != null) //user does not exist
            {
                _notice.AddErrors(ModelState);
                return View(nameof(Create));
            }

            var info = new UserLoginInfo("Email", userEmpty.Email, "Email");

            var createResult = await _userManager.CreateAsync(userEmpty);
            if (createResult.Succeeded)
            {
                var addLoginResult = await _userManager.AddLoginAsync(userEmpty, info);
                if (addLoginResult.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userEmpty.UserName); // This works because usernames are unique
                    var makeAdminResult = await _userManager.AddToRolesAsync(user, new[] {Constants.ROTA_ROLE, Constants.WOLF_ROLE });

                    await _events.AddEvent(AuthEventType.Register, JsonConvert.SerializeObject(new
                    {
                        LoginProvider = info?.LoginProvider ?? "Email",
                        ProviderKey = info?.ProviderKey ?? email
                    }), user);

                    //Register with application
                    try
                    {
                        var createBarcoUserResult = await new HttpClient().PostAsync("http://localhost:8000/api/BarcoMembers", new JsonContent(new
                        {
                            Name = user.FullName,
                            NickName = user.NickName,
                            UserName = user.UserName,
                            RotaStatus = barcoMember.RotaStatus
                        }));
                    }
                    catch (Exception)
                    {
                        //mute
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notice.AddErrors(ModelState, addLoginResult);
                }
            }
            else
            {
                _notice.AddErrors(ModelState, createResult);
            }

            await _userManager.DeleteAsync(userEmpty); // TODO: make atomic
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MakeRota(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.AddToRoleAsync(user, Constants.ROTA_ROLE);

            return _notice.Success(this, $"{user.UserName} is now a Rota member", $"{user.UserName} has to logout and login to activate the role");
        }

        [HttpGet]
        public async Task<IActionResult> MakeBarco(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            await _userManager.AddToRolesAsync(user, new[] { Constants.BARCO_ROLE, Constants.ROTA_ROLE });

            return _notice.Success(this, $"{user.UserName} is now a Barco member", $"{user.UserName} has to logout and login to activate the role");
        }
    }

    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
        { }
    }
}
