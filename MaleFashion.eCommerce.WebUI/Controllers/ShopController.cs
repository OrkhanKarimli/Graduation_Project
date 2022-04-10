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
    public class ShopController : Controller
    {
        readonly FashionDbContext db;

        public ShopController(FashionDbContext db)
        {
            this.db = db;
        }

        [AllowAnonymous]
        async public Task<IActionResult> Index(string searchShop, string color, string productTag, string brandName, string category, string size, int max, int min = 0)
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
                                                              ProductId = p.ProductId,
                                                              Title = p.Product.Title,
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

            if (!string.IsNullOrWhiteSpace(searchShop))
            {
                query = query.Where(q => q.Title.Contains(searchShop, StringComparison.OrdinalIgnoreCase));
            }
            else if (max != 0)
            {
                query = query.Where(q => q.PriceNew != null ? q.PriceNew > min && q.PriceNew < max : q.Price > min && q.Price < max);
            }
            else if (!string.IsNullOrWhiteSpace(color))
            {
                query = query.Where(q => q.ColorName.Equals(color));
            }
            else if (!string.IsNullOrWhiteSpace(productTag))
            {
                query = query.Where(q => q.ProductTagName.Equals(productTag));
            }
            else if (!string.IsNullOrWhiteSpace(brandName))
            {
                query = query.Where(q => q.Brand.Equals(brandName));
            }
            else if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(q => q.CategoryName.Equals(category));
            }
            else if (!string.IsNullOrWhiteSpace(size))
            {
                query = query.Where(q => q.SizeName.Equals(size));
            }

            viewModel.DiscountProductViewModel = query.ToList();
            viewModel.Colors = await db.Colors.ToListAsync();
            viewModel.Sizes = await db.Sizes.ToListAsync();
            viewModel.Categories = await db.Categories.ToListAsync();
            viewModel.ProductTags = await db.ProductTags.ToListAsync();
            viewModel.Brands = await db.Brands.ToListAsync();

            return View(viewModel);

            //return View();
        }
    }
}
