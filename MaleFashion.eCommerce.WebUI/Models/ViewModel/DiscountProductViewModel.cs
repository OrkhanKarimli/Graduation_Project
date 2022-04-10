using MaleFashion.eCommerce.WebUI.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.ViewModel
{
    public class DiscountProductViewModel
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public int BrandId { get; set; }

        public decimal Price { get; set; }

        public decimal? PriceNew { get; set; }

        public string CampaignTitle { get; set; }

        public string CampaignDescription { get; set; }

        public string ColorName { get; set; }

        public string SizeName { get; set; }

        public string ProductTagName { get; set; }

        public string CategoryName { get; set; }

        public decimal? Discount { get; set; }

        public IEnumerable<ProductImage> ProductImages { get; set; }

        public IEnumerable<Color> Colors { get; set; }

        public IEnumerable<Size> Sizes { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ProductTag> ProductTags { get; set; }
    }
}
