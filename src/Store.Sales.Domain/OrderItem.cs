using Store.Core.DomainObjects;
using System;

namespace Store.Sales.Domain
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            if (quantity < Order.MIN_ITEM_UNITS) throw new DomainException(message: $"Minimmum of {Order.MIN_ITEM_UNITS} units.");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        internal void AddUnits(int units)
        {
            Quantity += units;
        }

        internal decimal ComputePrice()
        {
            return Quantity * UnitPrice;
        }
    }
}
