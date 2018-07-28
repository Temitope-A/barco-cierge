using Microsoft.AspNetCore.Identity;
using Cierge.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cierge.Models.ManageViewModels
{
    public class HistoryViewModel
    {
        public IList<AuthEvent> Events { get; set; }
    }
}
