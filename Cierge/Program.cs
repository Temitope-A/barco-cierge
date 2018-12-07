using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Core;
using OpenIddict.Models;

namespace Cierge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            // Don't initialise database if given the IgnoreInitDb argument
            var ignoreInitDb = args.FirstOrDefault(a => a.ToLower().Contains("ignoreinitdb"));
            if (ignoreInitDb == null)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        InitializeAsync(services, CancellationToken.None).GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }
            }


            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();


        private static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Add OpenIddict clients
                var iddictManager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await iddictManager.FindByClientIdAsync("barco-rota", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "barco-rota",
                        DisplayName = "Barco Rota",
                        PostLogoutRedirectUris = { new Uri("https://barcorota.azurewebsites.net/signout-oidc") },
                        RedirectUris = { new Uri("https://barcorota.azurewebsites.net/signin-oidc") },
                    };

                    await iddictManager.CreateAsync(descriptor, cancellationToken);
                }

                // Create roles
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { Constants.ADMIN_ROLE, Constants.BARCO_ROLE, Constants.ROTA_ROLE, Constants.WOLF_ROLE };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
        
    }
}
