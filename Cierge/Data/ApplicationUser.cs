using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Cierge.Models;

namespace Cierge.Data
{
    public class ApplicationUser : IdentityUser, IAdditionalUserInfo
    {
        public IList<AuthEvent> Events { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        // from IAdditionalUserInfo

        public string FullName { get; set; }

        public string NickName { get; set; }

        public int PinCode { get; set; }
    }
}
