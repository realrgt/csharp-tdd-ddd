using MediatR;
using Store.Core.Messages;
using System;

namespace Store.Sales.Application.Events
{
    public class OrderItemAddedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        public OrderItemAddedEvent(Guid customerId, Guid orderId, Guid productId, string productName, decimal unitPrice, int quantity)
        {
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
    }
}
