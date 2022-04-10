using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class ProductTag : BaseEntity
    {
        public string ProductTagName { get; set; }

        public virtual ICollection<ProductMainCollection> ProductMainCollections { get; set; }
    }
}
