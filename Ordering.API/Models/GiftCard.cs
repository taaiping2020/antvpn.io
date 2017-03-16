using Ordering.API.SeedWork;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Models
{
    public class GiftCard
    {
        public string GiftCardKey { get; set; }
        public Subscription Subscription { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool Used { get; set; }
        //public Order Order { get; private set; }
    }
}
