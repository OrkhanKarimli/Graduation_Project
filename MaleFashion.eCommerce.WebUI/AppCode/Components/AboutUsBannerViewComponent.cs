using MaleFashion.eCommerce.WebUI.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Components
{
    public class AboutUsBannerViewComponent : ViewComponent
    {
        private readonly FashionDbContext db;

        public AboutUsBannerViewComponent(FashionDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {

            var data = db.AboutUsBanners.FirstOrDefault();

            return View(data);
        }
    }
}
