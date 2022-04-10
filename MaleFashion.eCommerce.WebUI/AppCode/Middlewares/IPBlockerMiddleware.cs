using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class IPBlockerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration conf;

        public IPBlockerMiddleware(RequestDelegate next, IConfiguration conf)
        {
            _next = next;
            this.conf = conf;
        }

        public Task Invoke(HttpContext httpContext)
        {
            string myIP = httpContext.Connection.RemoteIpAddress.ToString();

            string[] IPs = conf.GetValue<string>("BannedIPs")
                           .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (Array.IndexOf(IPs, myIP) != -1)
            {
                httpContext.Response.WriteAsync("Sizin IP unvaniniz bloklanib!");
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class IPBlockerMiddlewareExtensions
    {
        public static IApplicationBuilder UseIPBlockerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IPBlockerMiddleware>();
        }
    }
}
