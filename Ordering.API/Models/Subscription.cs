using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Models
{
    public class Subscription
    {
        public string Name { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public SubscriptionService SubscriptionService { get; set; }
        public static IEnumerable<Subscription> GetAllSubscription()
        {
            yield return new Subscription { Name = "12 Month Membership", TimeSpan = TimeSpan.FromDays(365 / 1), SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "6 Month Membership", TimeSpan = TimeSpan.FromDays(365 / 2), SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "3 Month Membership", TimeSpan = TimeSpan.FromDays(365 / 4), SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "1 Month Membership", TimeSpan = TimeSpan.FromDays(365 / 12), SubscriptionService = SubscriptionService.Vpn };
        }
    }
}
