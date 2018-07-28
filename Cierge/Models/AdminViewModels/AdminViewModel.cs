using Cierge.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cierge.Models.AdminViewModels
{
    public class AdminViewModel
    {
        public int UserCount { get; set; }

        [Display(Name ="Search users...")]
        public string SearchTerm { get; set; }

        public IList<ApplicationUser> Users { get; set; }
    }
}