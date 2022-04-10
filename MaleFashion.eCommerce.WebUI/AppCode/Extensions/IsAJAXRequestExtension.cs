using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }

            return false;
        }
    }
}
