using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Models.FormModel
{
    public class PersonalSideFormModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ImagePath { get; set; }

        public string JobName { get; set; }

        public string City { get; set; }

        public int Age { get; set; }
    }
}
