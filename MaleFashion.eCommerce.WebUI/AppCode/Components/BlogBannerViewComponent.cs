using MaleFashion.eCommerce.WebUI.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MaleFashion.eCommerce.WebUI.AppCode.Components
{
    public class BlogBannerViewComponent : ViewComponent
    {
        private readonly FashionDbContext db;

        public BlogBannerViewComponent(FashionDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {

            var data = db.BlogBanners.FirstOrDefault();

            return View(data);
        }
    }
}
