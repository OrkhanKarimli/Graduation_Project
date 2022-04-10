using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }

        public virtual ICollection<BlogDetailsTagsCollection> BlogDetailsTagsCollections { get; set; }
    }
}
