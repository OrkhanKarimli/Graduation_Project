using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Areas.Admin.Models.FormModel
{
    public class ResetPasswordFormModel
    {
        [Required(ErrorMessage = "Bu xana boş qoyulmamalıdır!")]
        [MinLength(8, ErrorMessage = "Minimum simvol sayı 8 olmalıdır!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu xana boş qoyulmamalıdır!")]
        [Compare("Password", ErrorMessage = "Şifrələr eyni olmalıdır!")]
        [MinLength(8, ErrorMessage = "Minimum simvol sayı 8 olmalıdır!")]
        public string PasswordConfirm { get; set; }

        public string Token { get; set; }

        public string Email { get; set; }
    }
}
