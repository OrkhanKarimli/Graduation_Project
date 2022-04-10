using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI
{
    public class Program
    {
        public static string[] principals { get; set; }

        public static void Main(string[] args)
        {
            Type[] types = typeof(Program).Assembly.GetTypes();

            principals = types
                         .Where(t => typeof(Controller).IsAssignableFrom(t)
                         && t.IsDefined(typeof(AuthorizeAttribute), true))
                         .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>())
                         .Union(types
                         .Where(t => typeof(Controller).IsAssignableFrom(t))
                         .SelectMany(type => type.GetMethods())
                         .Where(method => method.IsPublic
                         && !method.IsDefined(typeof(NonActionAttribute))
                         && method.IsDefined(typeof(AuthorizeAttribute)))
                         .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>()))
                         .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
                         .SelectMany(a => a.Policy.Split(new[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
                         .Distinct()
                         .ToArray();

            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
            try
            {
                Log.Information("Proqram işə düşür.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Proqram işə düşən zaman xəta baş verdi.");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() //Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
