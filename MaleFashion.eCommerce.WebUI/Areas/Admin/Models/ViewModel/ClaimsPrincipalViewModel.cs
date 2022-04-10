using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Models.ViewModel
{
    public class ClaimsPrincipalViewModel
    {
        public ClaimsPrincipalViewModel()
        {
            Principals = new Dictionary<string, bool>();
        }

        public AppUser User { get; set; }

        public Dictionary<string, bool> Principals { get; set; }
    }
}
