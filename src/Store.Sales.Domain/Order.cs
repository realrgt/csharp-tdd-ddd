using System.Collections.Generic;
using System.Linq;

namespace Store.Sales.Domain
{
    public class Order
    {
        public decimal TotalAmount { get; private set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public void AddItem(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
            TotalAmount = OrderItems.Sum(item => item.UnitPrice * item.Quantity);
        }
    }
}
