using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        public string EmailAddres { get; set; }

        [Required(ErrorMessage = "Bu xana doldurulmalıdır!")]
        public string Content { get; set; }

        public string Reply { get; set; }

        public DateTime? AnswerDate { get; set; }

        public DateTime SendDate { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
