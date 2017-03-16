using Ordering.API.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Models
{
    public class Order : Entity
    {
        public Order(int buyerId, int paymentMethodId, int subscriptionId)
        {
            _buyerId = buyerId;
            _orderStatusId = OrderStatus.InProcess.Id;
            _paymentMethodId = paymentMethodId;
            _orderDate = DateTimeOffset.UtcNow;
            _subscriptionId = subscriptionId;
        }
        private DateTimeOffset _orderDate;

        public Buyer Buyer { get; private set; }
        private int _buyerId;

        public OrderStatus OrderStatus { get; private set; }
        private int _orderStatusId;

        //public GiftCard GiftCard { get; set; }
        //public int _giftCardId { get; set; }

        public Subscription Subscription { get; private set; }
        private int _subscriptionId;

        public PaymentMethod PaymentMethod { get; private set; }
        private int _paymentMethodId;
    }
}
