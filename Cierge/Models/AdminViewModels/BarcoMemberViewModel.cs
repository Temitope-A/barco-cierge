using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cierge.Models.AdminViewModels
{
    public class BarcoMemberViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public int PinCode { get; set; }
        public RotaStatus RotaStatus { get; set; }
        public BarcoMemberViewModel()
        {
            RotaStatus = RotaStatus.Active;
        }
    }

    public enum RotaStatus
    {
        Inactive = 0,
        Active = 1
    }
}
