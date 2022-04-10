using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment env;

        public MonitoringMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            this.env = env;
        }

        public Task Invoke(HttpContext httpContext)
        {
            DateTime startDate = DateTime.UtcNow;

            return _next(httpContext).ContinueWith(t =>
            {
                using (var sw = new StreamWriter($"{env.WebRootPath}/log.txt", true))
                {
                    sw.WriteLine($"Başladı: {startDate:dd:MM:yyyy HH:mm:ss.fff}");
                    sw.WriteLine($"Host: {httpContext.Request.Host}");
                    sw.WriteLine($"Path: {httpContext.Request.Path}");
                    sw.WriteLine($"Method: {httpContext.Request.Method}");
                    sw.WriteLine($"QueryString: {httpContext.Request.QueryString}");

                    DateTime endDate = DateTime.UtcNow;

                    if (httpContext.Request.RouteValues.TryGetValue("area", out object area))
                    {
                        sw.WriteLine($"Area: {area}");
                    }

                    if (httpContext.Request.RouteValues.TryGetValue("controller", out object controller))
                    {
                        sw.WriteLine($"Controller: {controller}");
                    }

                    if (httpContext.Request.RouteValues.TryGetValue("action", out object action))
                    {
                        sw.WriteLine($"Action: {action}");
                    }

                    sw.WriteLine($"Bitdi: {endDate:dd:MM:yyyy HH:mm:ss.fff}\n");
                }
            });
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MonitoringMiddlewareExtensions
    {
        public static IApplicationBuilder UseMonitoringMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MonitoringMiddleware>();
        }
    }
}
