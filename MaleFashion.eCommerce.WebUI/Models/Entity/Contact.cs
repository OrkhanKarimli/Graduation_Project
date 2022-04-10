using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Contact : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
