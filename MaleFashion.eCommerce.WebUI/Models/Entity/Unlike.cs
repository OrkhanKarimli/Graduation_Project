using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Unlike
    {
        [Key]
        public int Id { get; set; }

        public int BlogId { get; set; }

        public int UserId { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual AppUser User { get; set; }
    }
}
