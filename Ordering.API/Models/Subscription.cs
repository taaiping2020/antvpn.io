using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Models
{
    public class Subscription
    {
        public string Name { get; set; }
        public AvailableTime AvailableTime { get; set; }
        public SubscriptionService SubscriptionService { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<GiftCard> GiftCards { get; set; }
        //public GiftCard GiftCard { get; set; }

        public static IEnumerable<Subscription> GetAllSubscription()
        {
            yield return new Subscription { Name = "12 Month Membership", AvailableTime = AvailableTime.Annual, SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "6 Month Membership", AvailableTime = AvailableTime.SemiAnnual, SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "3 Month Membership", AvailableTime = AvailableTime.Quarter, SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "1 Month Membership", AvailableTime = AvailableTime.Month, SubscriptionService = SubscriptionService.Vpn };
            yield return new Subscription { Name = "1 Week Membership", AvailableTime = AvailableTime.Week, SubscriptionService = SubscriptionService.Vpn };
        }
    }
}
