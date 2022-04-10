using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int AphorismId { get; set; }

        public string AuthorImagePath { get; set; }

        public string AuthorName { get; set; }

        public string AuthorSurname { get; set; }

        public virtual Aphorism Aphorism { get; set; }

        public virtual ICollection<BlogDetailsTagsCollection> BlogDetailsTagsCollections { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Unlike> Unlikes { get; set; }

        [NotMapped]

        public string ImageTemp { get; set; }
    }
}
