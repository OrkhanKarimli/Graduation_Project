using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class BlogDetailsTagsCollection
    {
        [Key]
        public int Id { get; set; }

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
