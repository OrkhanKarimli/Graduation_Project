using MaleFashion.eCommerce.WebUI.AppCode.Extensions;
using MaleFashion.eCommerce.WebUI.Areas.Admin.Models.FormModel;
using MaleFashion.eCommerce.WebUI.Areas.Admin.Models.ViewModel;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly FashionDbContext db;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration conf;

        public AccountController(FashionDbContext db, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration conf)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.conf = conf;
        }

        //----------------SIGN-OUT----------------

        async public Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index), "Home", new
            {
                area = ""
            });
        }

        //----------------SIGN-OUT----------------

        //----------------RESET-PASSWORD----------------

        [AllowAnonymous]
        public IActionResult ChangeOrForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        async public Task<IActionResult> ChangeOrForgotPassword(ChangeOrForgotPasswordFormModel formModel)
        {
            string email = formModel.Email;

            AppUser userResult = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

            if (userResult == null)
            {
                TempData["EmailUser"] = "Daxil etdiyiniz mail üzrə istifadəçimiz yoxdur!";
            }
            else
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(userResult);

                string url = Url.Action("ResetPassword", "Account", values: new
                {
                    email = email,
                    token = token
                }, Request.Scheme);

                SmtpClient client = new SmtpClient()
                {
                    Port = 25,
                    Host = "smtp.mail.ru",
                    EnableSsl = true
                };

                client.Credentials = new NetworkCredential(conf["SubsSMTP:FromMail"], conf["SubsSMTP:Pwd"]);

                MailMessage message = new MailMessage(conf["SubsSMTP:FromMail"], email);
                message.Subject = "Şifrə yeniləmə linki.";
                message.Body = $"Bu <a href={url}>linkə</a> click edərək keçid linkinə yollana bilərsiniz!";
                message.IsBodyHtml = true;

                client.Send(message);

                Thread.Sleep(2000);

                return RedirectToAction(nameof(ChangeOrForgotPasswordSent));
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult ChangeOrForgotPasswordSent()
        {

            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("ResetError", "Xəta baş verdi!");

                //return NotFound();
            }
            else
            {
                ResetPasswordFormModel formModel = new ResetPasswordFormModel();

                formModel.Email = email;
                formModel.Token = token;

                return View(formModel);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        async public Task<IActionResult> ResetPassword(ResetPasswordFormModel formModel)
        {
            AppUser userResult = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(formModel.Email));

            string token = formModel.Token;

            IdentityResult tokenResult = await userManager.ResetPasswordAsync(userResult, token, formModel.Password);

            if (tokenResult.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordSent));
            }
            else
            {
                foreach (IdentityError error in tokenResult.Errors)
                {
                    ModelState.AddModelError("ResetPasswordPostError", error.Description);
                }
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordSent()
        {

            return View();
        }

        //----------------RESET-PASSWORD----------------

        //----------------CONFIRM-EMAIL----------------

        public IActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        async public Task<IActionResult> ConfirmEmail(ConfirmEmailFormModel formModel)
        {
            string email = formModel.Email;

            AppUser user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                string confirmEmailLink = Url.Action("SetEmailConfirmed", "Account", new
                {
                    email = email,
                    token = token
                }, Request.Scheme);

                //using (var sw = new StreamWriter("token.txt"))
                //{
                //    sw.WriteLine(confirmEmailLink);
                //}

                SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.mail.ru",
                    Port = 25,
                    EnableSsl = true
                };
                client.Credentials = new NetworkCredential(conf["SubsSMTP:FromMail"], conf["SubsSMTP:Pwd"]);

                MailMessage message = new MailMessage(conf["SubsSMTP:FromMail"], email);
                message.Subject = "Email təsdiqləmə linki.";
                message.Body = $"<a href='{confirmEmailLink}'>Bura</a> klik edərək email təsdiqləmə pəncərəsinə yönələ bilərsiniz.";
                message.IsBodyHtml = true;
                client.Send(message);

                return RedirectToAction("ConfirmEmailSent", "Account");
            }
            else
            {
                TempData["ConfirmEmailError"] = "Belə bir istifadəçi yoxdur!";
            }

            return View();
        }

        public IActionResult ConfirmEmailSent()
        {
            return View();
        }

        [AllowAnonymous]
        async public Task<IActionResult> SetEmailConfirmed(string email, string token)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError("SetEmailConfirmedError", "Xəta baş verdi!");
            }
            else
            {
                AppUser user = await userManager.FindByEmailAsync(email);
                IdentityResult emailConfirmresult = await userManager.ConfirmEmailAsync(user, token);

                if (emailConfirmresult.Succeeded)
                {
                    return View();
                }
                else
                {
                    foreach (IdentityError error in emailConfirmresult.Errors)
                    {
                        ModelState.AddModelError("SetEmailConfirmedError", error.Description);
                    }
                }
            }

            return View();
        }

        //----------------CONFIRM-EMAIL----------------

        //----------------CLAIMS-VIEW----------------

        public IActionResult ViewUserClaims()
        {
            var viewModel = new ViewClaimsViewModel();

            IEnumerable<AppUser> currentUsers = db.Users.ToList();

            viewModel.Users = currentUsers;

            return View(viewModel);
        }

        //----------------CLAIMS-VIEW----------------

        //----------------CHANGE-REAL-TIME-CLAIM----------------

        [HttpPost]
        [Authorize(Policy = "admin.setprincipal")]
        async public Task<IActionResult> SetUserPrincipal(int userId, string principalName, bool applied)
        {
            if (userId <= 0)
            {
                return Json(new
                {
                    error = true,
                    message = "Istifadechi movcud deyil."
                });
            }

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Istifadechi movcud deyil."
                });
            }

            if (applied == true)
            {
                await db.UserClaims.AddAsync(new AppUserClaim
                {
                    UserId = userId,
                    ClaimType = principalName,
                    ClaimValue = "1"
                });

                await db.SaveChangesAsync();
            }
            else
            {
                var currentClaim = await db.UserClaims.AsNoTracking().FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClaimType == principalName);

                if (currentClaim != null)
                {
                    db.UserClaims.Remove(currentClaim);

                    await db.SaveChangesAsync();
                }
                else
                {
                    return Json(new
                    {
                        error = true,
                        message = "Selahiyyet tapilmadi."
                    });
                }
            }

            return RedirectToAction("GetUserPrincipal", routeValues: new
            {
                id = userId
            });
        }

        [Authorize(Policy = "admin.getprincipal")]
        public IActionResult GetUserPrincipal(int id)
        {
            var viewModel = new ClaimsPrincipalViewModel();

            var user = db.Users.FirstOrDefault(u => u.Id == id);

            viewModel.User = user;

            var claimsOfUser = db.UserClaims.Where(uc => uc.UserId == id).ToList();

            string[] principals = Program.principals;

            foreach (string principal in principals)
            {
                if (claimsOfUser.Any(cou => cou.ClaimType == principal && cou.ClaimValue == "1"))
                {
                    viewModel.Principals.Add(principal, true);
                }
                else
                {
                    viewModel.Principals.Add(principal, false);
                }
            }

            return View(viewModel);
        }
    }
}