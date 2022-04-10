using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Size : BaseEntity
    {
        public string SizeName { get; set; }

        public virtual ICollection<ProductMainCollection> ProductMainCollections { get; set; }
    }
}
