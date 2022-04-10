using MaleFashion.eCommerce.WebUI.Models.FormModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Product : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }

        public virtual ICollection<ProductMainCollection> ProductMainCollections { get; set; }

        [NotMapped]
        public ImageItemFormModel[] Files { get; set; }
    }
}
