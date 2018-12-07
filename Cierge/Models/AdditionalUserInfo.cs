using System.ComponentModel.DataAnnotations;

namespace Cierge.Models
{
    public interface IAdditionalUserInfo
    {
        string UserName { get; set; }
        string FullName { get; set; }
        string NickName { get; set; }
        int PinCode { get; set; }
    }

    public class AdditionalUserInfo : IAdditionalUserInfo
    {
        [Required]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Your username should be between 4 and 15 characters in length.")]
        [Display(Name = "Username", Prompt = "unique, short, no spaces")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Name", Prompt = "optional full name")]
        [StringLength(20, ErrorMessage = "Your name can't be more than 20 characters.")]
        public string FullName { get; set; }

        [Display(Name = "Nickname", Prompt = "optional")]
        [StringLength(10, ErrorMessage = "Your nick name can't be more than 10 characters.")]
        public string NickName { get; set; }

        [Display(Name = "Pincode", Prompt = "required")]
        public int PinCode { get; set; }

    }
}
