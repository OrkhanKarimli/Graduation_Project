using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Models.ViewModel
{
    public class ViewClaimsViewModel
    {
        public IEnumerable<AppUser> Users { get; set; }
    }
}
