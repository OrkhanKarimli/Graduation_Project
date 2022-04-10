using MaleFashion.eCommerce.WebUI.Models.Entity.Membership;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Reply
    {
        [Key]
        public int Id { get; set; }

        public int BlogId { get; set; }

        public int CommentId { get; set; }

        public int UserId { get; set; }

        public string Content { get; set; }

        public string AuthorImagePath { get; set; }

        public string AuthorName { get; set; }

        public string AuthorSurname { get; set; }

        public DateTime ReplyDate { get; set; } = DateTime.UtcNow.AddHours(4);

        public virtual Comment Comment { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual AppUser User { get; set; }
    }
}
