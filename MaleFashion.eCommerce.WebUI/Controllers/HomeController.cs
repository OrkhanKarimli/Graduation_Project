using MaleFashion.eCommerce.WebUI.Models.DataContext;
using MaleFashion.eCommerce.WebUI.Models.Entity;
using MaleFashion.eCommerce.WebUI.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly FashionDbContext db;

        public HomeController(FashionDbContext db)
        {
            this.db = db;
        }

        [AllowAnonymous]
        async public Task<IActionResult> Index()
        {
            // TempData Data Transfer To Contact Controller's Index Action
            List<int> numbers = new List<int>() { 13, 23, 77, 93 };

            TempData["Data"] = JsonConvert.SerializeObject(numbers);
            // TempData Data Transfer To Contact Controller's Index Action

            CookieOptions options = new CookieOptions();
            options.Expires = new DateTimeOffset(DateTime.Now.AddMinutes(13)); ;
            HttpContext.Response.Cookies.Append("Welcome-Cookie", "TRUE", options);

            HttpContext.Session.SetInt32("Welcome-Session", 13);

            ShopViewModel viewModel = new ShopViewModel();

            IEnumerable<ProductMainCollection> productCollecion = db.ProductMainCollections
                                                                  .Include(p => p.Product)
                                                                  .Include(ca => ca.Category)
                                                                  .Include(co => co.Color)
                                                                  .Include(s => s.Size)
                                                                  .Include(pt => pt.ProductTag)
                                                                  .Include(pb => pb.Product.Brand)
                                                                  .Include(pi => pi.Product.ProductImages);

            DateTime dt = DateTime.UtcNow.AddHours(4);

            IEnumerable<ProductCampaignCollection> campaignCollection = db.ProductCampaignCollections
                                                                        .Include(pc => pc.Campaign)
                                                                        .Where(pc => pc.Campaign.IsApproved && pc.Campaign.ExpiredDate > dt);

            IQueryable<DiscountProductViewModel> query = (from p in productCollecion
                                                          join cp in campaignCollection on p.Id equals cp.ProductCollectionId
                                                          into pcp
                                                          from pcp_item in pcp.DefaultIfEmpty()
                                                          select new DiscountProductViewModel
                                                          {
                                                              Id = p.Id,
                                                              Title = p.Product.Title,
                                                              Description = p.Product.Description,
                                                              Brand = p.Product.Brand.BrandName,
                                                              BrandId = p.Product.BrandId,
                                                              Price = p.Price,
                                                              PriceNew = pcp_item?.PriceNew == null ? null : pcp_item?.PriceNew,
                                                              CampaignTitle = pcp_item?.Campaign?.Title,
                                                              CampaignDescription = pcp_item?.Campaign?.Description,
                                                              Discount = pcp_item?.Campaign?.Discount == null ? null : pcp_item?.Campaign?.Discount,
                                                              ProductImages = p.Product.ProductImages
                                                          })
                                                          .AsQueryable();

            IEnumerable<DiscountProductViewModel> data = query.ToList();

            viewModel.DiscountProductViewModel = data;
            viewModel.Colors = await db.Colors.ToListAsync();
            viewModel.Sizes = await db.Sizes.ToListAsync();
            viewModel.Categories = await db.Categories.ToListAsync();
            viewModel.ProductTags = await db.ProductTags.ToListAsync();
            viewModel.Brands = await db.Brands.ToListAsync();

            return View(viewModel);
        }
    }
}
