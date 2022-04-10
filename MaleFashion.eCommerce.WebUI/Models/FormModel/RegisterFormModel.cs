using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.FormModel
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        [Compare("Password", ErrorMessage = "Şifrələr eyni olmalıdır!")]
        public string PasswordConfirm { get; set; }
    }
}
