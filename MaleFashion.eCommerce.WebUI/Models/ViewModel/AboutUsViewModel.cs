using MaleFashion.eCommerce.WebUI.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.ViewModel
{
    public class AboutUsViewModel
    {
        public IEnumerable<WhyWe> WhyWes { get; set; }

        public IEnumerable<Blog> Blogs { get; set; }

        public IEnumerable<Team> Teams { get; set; }

        public IEnumerable<TeamJob> TeamJobs { get; set; }

        public IEnumerable<HappyClient> HappyClients { get; set; }
    }
}
