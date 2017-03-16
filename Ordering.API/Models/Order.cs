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

        public GiftCard GiftCard { get; private set; }

        public Subscription Subscription { get; private set; }
        private int _subscriptionId;

        public PaymentMethod PaymentMethod { get; private set; }
        private int _paymentMethodId;

        public void CompleteOrder(GiftCard giftCard)
        {
            if (this.OrderStatus != OrderStatus.InProcess)
            {
                throw new InvalidOperationException("OrderStatus is invalid.");
            }
            if (giftCard.Used == true)
            {
                throw new ArgumentException("GiftCard is used, try add new one.");
            }

            this.GiftCard = giftCard;
            giftCard.Used = true;
           
        }
    }
}
