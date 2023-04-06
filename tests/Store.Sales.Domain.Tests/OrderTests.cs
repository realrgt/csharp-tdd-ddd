using System;
using System.Linq;
using Xunit;

namespace Store.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add New Item Order")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(expected: 200, actual: order.TotalAmount);
        }

        [Fact(DisplayName = "Add Item Existing Order")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_ExistingItem_ShouldIncrementUnitsSomePrices()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);

            // Act
            order.AddItem(orderItem2);

            // Assert
            Assert.Equal(expected: 300, actual: order.TotalAmount);
            Assert.Equal(expected: 1, actual: order.OrderItems.Count);
            Assert.Equal(expected: 3, actual: order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
        }
    }
}
