using MaleFashion.eCommerce.WebUI.AppCode.BinderProviders;
using MaleFashion.eCommerce.WebUI.AppCode.Hubs;
using MaleFashion.eCommerce.WebUI.AppCode.Providers;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;

namespace MaleFashion.eCommerce.WebUI
{
    public class Startup
    {
        private readonly IConfiguration conf;

        public Startup(IConfiguration conf)
        {
            this.conf = conf;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                //options.LowercaseQueryStrings = true;
            });

            services.AddSignalR();

            services.AddSession(requirements =>
            {
                // Enner Valencia - 13
                requirements.IdleTimeout = TimeSpan.FromMilliseconds(13);
            });

            services.AddMediatR(this.GetType().Assembly /*typeof(Startup)*/);

            services.AddControllersWithViews(cfg =>
            {
                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());

                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();

                cfg.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); ;


            services.AddDbContext<FashionDbContext>(cfg =>
            {
                cfg.UseSqlServer(conf.GetConnectionString("cString"));
            });

            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<FashionDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<UserManager<AppUser>>()
                     .AddScoped<RoleManager<AppRole>>()
                     .AddScoped<SignInManager<AppUser>>();

            services.AddScoped<IClaimsTransformation, ClaimsTransformationProvider>();

            services.Configure<IdentityOptions>(options =>
            {
                // şifrə tənzimləmələri.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Kilidləmə tənzimləri.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // istifadəçi adı tənzimləri.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(13);

                options.LoginPath = "/signin.html";
                options.AccessDeniedPath = "/accessdenied.html";
                options.SlidingExpiration = true;

                // call cookie
                options.Cookie.Name = ".MaleFashion.eCommerce.Cookie.Analyisers";
                // against cross-site attacks (even if user b has the cookie information of user a, user b cannot do anything, because location)
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            services.AddAuthentication();
            //-------------------------------------------
            services.AddAuthorization(options =>
            {
                string[] principals = Program.principals;

                foreach (string principal in principals)
                {
                    options.AddPolicy(principal, cfg =>
                    {
                        cfg.RequireClaim(principal, "1");
                    });
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // My IP Blocker Middleware
            //app.UseIPBlockerMiddleware();

            //app.UseMonitoringMiddleware();

            app.UseRequestLocalization(cfg =>
            {
                cfg.AddSupportedCultures("en", "az", "ru");
                cfg.AddSupportedUICultures("en", "az", "ru");
                cfg.RequestCultureProviders.Clear(); // Clears all the default culture providers from the list
                cfg.RequestCultureProviders.Add(new AppCultureProvider());
            });


            app.UseSession();

            app.DataSeed();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "SignInRoute",
                    pattern: "signin.html",
                    defaults: new
                    {
                        controller = "Account",
                        action = "SignIn"
                    });

                endpoints.MapControllerRoute(name: "RegisterRoute",
                    pattern: "register.html",
                    defaults: new
                    {
                        controller = "Account",
                        action = "Register"
                    });

                endpoints.MapControllerRoute(name: "AccessDeniedRoute",
                    pattern: "accessdenied.html",
                    defaults: new
                    {
                        controller = "Account",
                        action = "AccessDenied"
                    });

                endpoints.MapAreaControllerRoute(name: "adminArea",
                    areaName: "admin",
                    pattern: "admin/{controller=Products}/{action=Index}/{id?}");

                // SignalR Configuration To Route
                endpoints.MapHub<ConversationHub>("/chat");

                // Defaults, Liar Routes
                endpoints.MapControllerRoute(name: "BlogRoute",
                    pattern: "blog.html",
                    defaults: new
                    {
                        controller = "Blog",
                        action = "Index"
                    });

                // Route Constraints
                //endpoints.MapControllerRoute(name: "BlogRoute",
                //   pattern: "{controller=Home}/{action=Index}/{id?}",
                //   //pattern: "{controller=Home}/{action=Index}/{id:int:min(1)}",
                //   constraints: new
                //   {
                //       id = new IRouteConstraint[]
                //       {
                //          new MinLengthRouteConstraint(1)
                //       }
                //   });

                endpoints.MapControllerRoute(name: "default",
                                    pattern: "{lang=temp}/{controller=Home}/{action=Index}/{id?}",
                                    constraints: new
                                    {
                                        lang = "az|en|ru|temp"
                                    });
            });
        }
    }
}
