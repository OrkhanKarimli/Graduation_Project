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
    public class ShopDetailsController : Controller
    {
        private readonly FashionDbContext db;

        public ShopDetailsController(FashionDbContext db)
        {
            this.db = db;
        }

        [AllowAnonymous]
        public IActionResult Index(int? id)
        {
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
                                                              ProductId = p.ProductId,
                                                              Description = p.Product.Description,
                                                              Brand = p.Product.Brand.BrandName,
                                                              BrandId = p.Product.BrandId,
                                                              Price = p.Price,
                                                              PriceNew = pcp_item?.PriceNew == null ? null : pcp_item?.PriceNew,
                                                              ColorName = p.Color.ColorName,
                                                              SizeName = p.Size.SizeName,
                                                              CategoryName = p.Category.Name,
                                                              ProductTagName = p.ProductTag.ProductTagName,
                                                              CampaignTitle = pcp_item?.Campaign?.Title,
                                                              CampaignDescription = pcp_item?.Campaign?.Description,
                                                              Discount = pcp_item?.Campaign?.Discount == null ? null : pcp_item?.Campaign?.Discount,
                                                              ProductImages = p.Product.ProductImages
                                                          })
                                                          .AsQueryable();

            IEnumerable<DiscountProductViewModel> data = query
                                                         .Where(q => q.ProductId.Equals(id))
                                                         .ToList();

            return View(data);
        }
    }
}
