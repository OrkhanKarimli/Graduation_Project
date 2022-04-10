using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class AppInfo : BaseEntity
    {
        public string HeaderLogoPath { get; set; }

        public string FooterLogoPath { get; set; }

        public string Description { get; set; }

        public string CardsLogoPath { get; set; }

        public string ContactTitle { get; set; }

        public string ContactDescription { get; set; }

        public string FooterSiteInfo { get; set; }

        [NotMapped]
        public string HeaderImgTemp { get; set; }

        [NotMapped]
        public string FooterImgTemp { get; set; }

        [NotMapped]
        public string CardImgTemp { get; set; }
    }
}
