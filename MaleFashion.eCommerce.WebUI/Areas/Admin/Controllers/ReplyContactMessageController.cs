using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReplyContactMessageController : Controller
    {
        readonly FashionDbContext db;
        readonly IConfiguration conf;

        public ReplyContactMessageController(FashionDbContext db, IConfiguration conf)
        {
            this.db = db;
            this.conf = conf;
        }

        async public Task<IActionResult> Index(bool visibility)
        {
            TempData["Vis"] = "";

            if (visibility)
            {
                IEnumerable<ContactMessage> answeredContactMessages = await db.ContactMessages
                                                                      .Where(cm => !string.IsNullOrWhiteSpace(cm.Reply))
                                                                      .ToListAsync();

                TempData["Vis"] = "checked";

                return View(answeredContactMessages);
            }
            else
            {
                IEnumerable<ContactMessage> notAnsweredContactMessages = await db.ContactMessages
                                                                         .Where(cm => string.IsNullOrWhiteSpace(cm.Reply))
                                                                         .ToListAsync();

                TempData["Vis"] = "";

                return View(notAnsweredContactMessages);
            }
        }

        async public Task<IActionResult> Reply(int? id)
        {
            ContactMessage notAnsweredContactMessages = await db.ContactMessages.FindAsync(id);

            return View(notAnsweredContactMessages);
        }

        [HttpPost]
        async public Task<IActionResult> Reply(int? id, ContactMessage message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(message);
            }

            if (ModelState.IsValid)
            {
                SmtpClient client = new SmtpClient()
                {
                    EnableSsl = true,
                    Host = "smtp.mail.ru",
                    Port = 25
                };

                client.Credentials = new NetworkCredential(conf.GetValue<string>("SubsSMTP:FromMail"), conf["SubsSMTP:Pwd"]);

                MailMessage mailMessage = new MailMessage(conf.GetValue<string>("SubsSMTP:FromMail"), message.EmailAddres);
                mailMessage.Subject = "Cavab - Male Fashion eCommerce";
                mailMessage.Body = $"<h3>Sizin sual</h3> <p>- {message.Content}</p>" +
                    $"<h3>Bizim cavab</h3> <p>- {message.Reply}</p>";
                mailMessage.IsBodyHtml = true;

                try
                {
                    client.Send(mailMessage);

                    message.AnswerDate = DateTime.UtcNow.AddHours(4);
                    db.ContactMessages.Update(message);
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return Json("Xəta baş verdi, biraz sonra yenidən yoxlayın!");
                }
            }

            return RedirectToAction(nameof(Index), "ReplyContactMessage");
        }
    }
}
