using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using MaleFashion.eCommerce.WebUI.Models.FormModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MaleFashion.eCommerce.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly FashionDbContext db;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AccountController(FashionDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        async public Task<IActionResult> SignIn(SignInFormModel formModel)
        {
            Regex pattern = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            AppUser userResult = null;

            if (pattern.IsMatch(formModel.Username))
            {
                userResult = await userManager.FindByEmailAsync(formModel.Username);
            }
            else
            {
                userResult = await userManager.FindByNameAsync(formModel.Username);
            }

            if (userResult != null)
            {
                SignInResult signInResult = await signInManager.PasswordSignInAsync(userResult, formModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    string returlUrl = HttpContext.Request.Query["returnUrl"];

                    if (!string.IsNullOrWhiteSpace(returlUrl))
                    {
                        return Redirect(returlUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), "PersonalSide", new
                        {
                            area = "Admin"
                        });
                    }
                }
                else
                {
                    ModelState.AddModelError("SignError", "İstifadəçi adı və ya şifrə səhvdir.");
                    TempData["SingInError"] = "İstifadəçi adı və ya şifrə səhvdir.";
                }
            }
            else
            {
                ModelState.AddModelError("SignError", "İstifadəçi adı və ya şifrə səhvdir.");
                TempData["SingInError"] = "İstifadəçi adı və ya şifrə səhvdir.";
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        async public Task<IActionResult> Register(RegisterFormModel formModel)
        {
            var appUser = new AppUser
            {
                UserName = formModel.Username,
                Email = formModel.Email
            };

            var appResponse = await userManager.CreateAsync(appUser, formModel.Password);

            if (appResponse.Succeeded)
            {
                return Redirect(@"\signin.html");
            }
            else
            {
                foreach (IdentityError error in appResponse.Errors)
                {
                    ModelState.AddModelError("RegisterError", error.Description);
                }
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            return View();
        }
    }
}
