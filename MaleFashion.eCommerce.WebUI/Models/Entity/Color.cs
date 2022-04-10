using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Color : BaseEntity
    {
        public string ColorName { get; set; }

        public virtual ICollection<ProductMainCollection> ProductMainCollections { get; set; }
    }
}
