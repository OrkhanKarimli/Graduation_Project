using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.FormModel
{
    public class SignInFormModel
    {
        [Required(ErrorMessage = "Xahiş olunur istifadəçi adınızı və ya email-inizi daxil edin!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Xahiş olunur şifrənizi daxil edin!")]
        [MinLength(8, ErrorMessage = "Minimum 8 simvol daxil olunmalıdır!")]
        public string Password { get; set; }
    }
}
