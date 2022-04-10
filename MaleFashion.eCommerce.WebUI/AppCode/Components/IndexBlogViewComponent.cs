using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Components
{
    public class IndexBlogViewComponent : ViewComponent
    {
        private readonly FashionDbContext db;

        public IndexBlogViewComponent(FashionDbContext db)
        {
            this.db = db;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<Blog> data = db.Blogs.OrderByDescending(b => b.Id).Take(3).ToList();

            return View(data);
        }
    }
}
