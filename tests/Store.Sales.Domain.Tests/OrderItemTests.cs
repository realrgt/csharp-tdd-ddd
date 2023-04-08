using Store.Core.DomainObjects;
using System;
using Xunit;

namespace Store.Sales.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "Add Order Item Under Allawed Quantity")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_ItemUnderAllowedQuantity_ShouldReturnException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(testCode: () => new OrderItem(Guid.NewGuid(), productName: "Test Product", quantity: Order.MIN_ITEM_UNITS - 1, unitPrice: 100));
        }
    }
}
