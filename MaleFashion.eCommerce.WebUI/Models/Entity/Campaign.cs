using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Campaign : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Discount { get; set; }

        public DateTime ExpiredDate { get; set; }

        public bool IsApproved { get; set; }

        public virtual ICollection<ProductCampaignCollection> ProductCampaignCollections { get; set; }
    }
}
