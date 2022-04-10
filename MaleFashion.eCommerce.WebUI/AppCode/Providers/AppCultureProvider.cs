using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Providers
{
    public class AppCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string lang = "az";

            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var match = Regex.Match(httpContext.Request.Path, @"\/(?<lang>en|az|ru)\/?.*");

            if (match.Success && !string.IsNullOrWhiteSpace(match.Groups["lang"].Value))
            {
                httpContext.Response.Cookies.Delete("lang");
                httpContext.Response.Cookies.Append("lang", match.Groups["lang"].Value, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(3)
                });
                return Task.FromResult(new ProviderCultureResult(match.Groups["lang"].Value));
            }

            if (httpContext.Request.Cookies.TryGetValue("lang", out lang))
                return Task.FromResult(new ProviderCultureResult(lang.ToString()));


            return Task.FromResult(new ProviderCultureResult(lang));
        }
    }
}
