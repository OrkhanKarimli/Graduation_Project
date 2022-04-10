using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class ProductImage
    {
        public int Id { get; set; }

        public bool IsMain { get; set; }

        public string ImagePath { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
