using MaleFashion.eCommerce.WebUI.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.ViewModel
{
    public class BlogViewModel
    {
        public PagedViewModel<Blog> PagedBlogViewModel { get; set; }
        public PagedViewModel<BlogDetailsTagsCollection> PagedBlogDetailsTagsCollectionViewModel { get; set; }
    }
}
