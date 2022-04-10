using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class ProductCampaignCollection
    {
        [Key]
        public int Id { get; set; }

        public int ProductCollectionId { get; set; }

        public virtual ProductMainCollection ProductCollection { get; set; }

        public int CampaignId { get; set; }

        public virtual Campaign Campaign { get; set; }

        public decimal? PriceNew { get; set; }
    }
}
