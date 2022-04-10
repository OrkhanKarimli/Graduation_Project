using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Department : BaseEntity
    {
        public string City { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
