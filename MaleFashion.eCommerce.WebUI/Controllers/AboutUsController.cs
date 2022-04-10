using MaleFashion.eCommerce.WebUI.Models.DataContext;
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
    public class AboutUsController : Controller
    {
        private readonly FashionDbContext db;

        public AboutUsController(FashionDbContext db)
        {
            this.db = db;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var viewModel = new AboutUsViewModel();

            viewModel.WhyWes = db.WhyWes.ToList();

            viewModel.Blogs = db.Blogs.ToList();

            viewModel.Teams = db.Teams
                              .Include(tj => tj.TeamJob)
                              .ToList();

            viewModel.TeamJobs = db.TeamJobs.ToList();

            viewModel.HappyClients = db.HappyClients.ToList();

            return View(viewModel);
        }
    }
}
