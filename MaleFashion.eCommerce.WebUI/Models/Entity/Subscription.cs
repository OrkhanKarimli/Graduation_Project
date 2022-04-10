using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.Models.Entity
{
    public class Subscription
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime SubscriptionDate { get; set; } = DateTime.UtcNow.AddHours(4);
    }
}
