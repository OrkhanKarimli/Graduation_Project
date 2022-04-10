using MaleFashion.eCommerce.WebUI.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.ViewModel
{
    public class BlogDetailsViewModel
    {
        public IEnumerable<BlogDetailsTagsCollection> BlogDetailsTagsCollections { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<Reply> Replies { get; set; }

        public IEnumerable<Like> Likes { get; set; }

        public IEnumerable<Unlike> Unlikes { get; set; }

        public Blog PrevBlog { get; set; }

        public Blog NextBlog { get; set; }
    }
}
