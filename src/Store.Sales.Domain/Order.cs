using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Sales.Domain
{
    public class Order
    {
        public Guid CustomerId { get; private set; }
        public decimal TotalAmount { get; private set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public OrderStatus OrderStatus { get; private set; }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public void ComputeOrderPrice()
        {
            TotalAmount = OrderItems.Sum(item => item.ComputePrice());
        }

        public void AddItem(OrderItem orderItem)
        {
            if (_orderItems.Any(p => p.ProductId == orderItem.ProductId))
            {
                var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
                existingItem.AddUnits(orderItem.Quantity);

                orderItem = existingItem;

                _orderItems.Remove(existingItem);
            }

            _orderItems.Add(orderItem);
            ComputeOrderPrice();
        }

        public void MakeDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public static class OrderFactory
        {
            public static Order NewDraftOrder(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                };

                order.MakeDraft();
                return order;
            }
        }

    }
}
