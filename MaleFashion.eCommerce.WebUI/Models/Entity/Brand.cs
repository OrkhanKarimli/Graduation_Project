using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
