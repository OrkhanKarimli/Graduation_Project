using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Controllers
{
    public class SubsController : Controller
    {
        private readonly IConfiguration conf;
        private readonly FashionDbContext db;

        public SubsController(IConfiguration conf, FashionDbContext db)
        {
            this.conf = conf;
            this.db = db;
        }

        [HttpPost]
        [AllowAnonymous]
        async public Task<IActionResult> Index(string subsMail)
        {
            bool subscriptionResult = await db.Subscriptions.AsNoTracking().AnyAsync(s => s.Email.Equals(subsMail));

            if (subscriptionResult)
            {
                return Json(new
                {
                    error = true,
                    message = "Siz artıq bizim abunəliyimizə qoşulmusunuz!"
                });
            }
            else
            {
                SmtpClient client = new SmtpClient()
                {
                    Port = 25,
                    Host = "smtp.mail.ru",
                    EnableSsl = true
                };

                client.Credentials = new NetworkCredential(conf["SubsSMTP:FromMail"], conf["SubsSMTP:Pwd"]);

                MailMessage message = new MailMessage(conf["SubsSMTP:FromMail"], subsMail);
                message.Subject = conf.GetValue<string>("SubsSMTP:Title");
                message.Body = conf.GetValue<string>("SubsSMTP:Description");

                Subscription obj = new Subscription
                {
                    Email = subsMail
                };

                try
                {
                    client.Send(message);

                    await db.Subscriptions.AddAsync(obj);
                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = "Müvəffəqiyyətlə bizim abunəliyimizə qoşuldunuz, həmçinin mail ünvanınıza təsdiq mesajı göndərildi!"
                    });
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        error = false,
                        message = "Xəta baş verdi, biraz sonra yenidən cəhd edin!"
                    });
                }
            }
        }
    }
}
