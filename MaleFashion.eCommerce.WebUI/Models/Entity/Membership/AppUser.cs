using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity.Membership
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string ImagePath { get; set; }

        public string City { get; set; }

        public string JobName { get; set; }

        public int Age { get; set; }

        [NotMapped]
        public string ImageTemp { get; set; }
    }
}
