using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Team : BaseEntity
    {
        public string ImagePath { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int TeamJobId { get; set; }

        public virtual TeamJob TeamJob { get; set; }

        [NotMapped]
        public string FileTemp { get; set; }
    }
}
