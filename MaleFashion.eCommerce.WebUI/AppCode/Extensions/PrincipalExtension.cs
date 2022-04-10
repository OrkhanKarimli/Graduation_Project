using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;

            int userId = Convert.ToInt32(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return userId;
        }
    }
}
