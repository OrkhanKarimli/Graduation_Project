using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class TeamJob : BaseEntity
    {
        public string JobName { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
