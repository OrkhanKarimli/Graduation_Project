using MaleFashion.eCommerce.WebUI.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.ViewModel
{
    public class ShopViewModel
    {
        public IEnumerable<Color> Colors { get; set; }

        public IEnumerable<Size> Sizes { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<ProductTag> ProductTags { get; set; }

        public IEnumerable<Brand> Brands { get; set; }

        public IEnumerable<DiscountProductViewModel> DiscountProductViewModel { get; set; }
    }
}
