using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Models.FormModel
{
    public class ConfirmEmailFormModel
    {
        [Required(ErrorMessage = "Bu xana boş qoyulmamalıdır!")]
        public string Email { get; set; }
    }
}
