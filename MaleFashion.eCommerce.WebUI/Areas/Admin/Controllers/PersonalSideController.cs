using MaleFashion.eCommerce.WebUI.AppCode.Extensions;
using MaleFashion.eCommerce.WebUI.Areas.Admin.Models.FormModel;
using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PersonalSideController : Controller
    {
        private readonly FashionDbContext db;
        private readonly IWebHostEnvironment env;
        private readonly UserManager<AppUser> userManager;

        public PersonalSideController(FashionDbContext db, UserManager<AppUser> userManager, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            AppUser user = db.Users.FirstOrDefault(u => u.Id == User.GetUserId());

            return View(user);
        }
    }
}
