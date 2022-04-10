using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Aphorism : BaseEntity
    {
        public string Content { get; set; }

        public string Author { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
