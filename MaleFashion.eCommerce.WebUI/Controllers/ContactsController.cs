using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using MaleFashion.eCommerce.WebUI.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Controllers
{
    public class ContactsController : Controller
    {
        private readonly FashionDbContext db;

        public ContactsController(FashionDbContext db)
        {
            this.db = db;
        }

        [AllowAnonymous]
        async public Task<IActionResult> Index()
        {
            var viewModel = new ContactAndMessageViewModel();

            Contact contact = await db.Contacts
                              .Include(d => d.Departments)
                              .FirstOrDefaultAsync();

            viewModel.Contact = contact;

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        async public Task<IActionResult> SendMessage(ContactAndMessageViewModel viewModel)
        {
            await db.ContactMessages.AddAsync(new ContactMessage
            {
                Name = viewModel.ContactMessage.Name,
                EmailAddres = viewModel.ContactMessage.EmailAddres,
                Content = viewModel.ContactMessage.Content
            });

            if (ModelState.IsValid)
            {
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }
    }
}
