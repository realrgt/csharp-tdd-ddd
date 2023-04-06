using System;
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
            var order = new Order();
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(expected: 200, actual: order.TotalAmount);
        }
    }
}
