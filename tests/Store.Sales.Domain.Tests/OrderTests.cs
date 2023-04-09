using Store.Core.DomainObjects;
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

        [Fact(DisplayName = "Add Order Item Above Allawed Quantity")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_ItemAboveAllowedQuantity_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, productName: "Test Product", quantity: Order.MAX_ITEM_UNITS + 1, unitPrice: 100);

            // Act & Assert
            Assert.Throws<DomainException>(testCode: () => order.AddItem(orderItem));
        }

        [Fact(DisplayName = "Add Existing Order Item Above Allawed Quantity")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_ItemAboveAllowedUnitsSum_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);
            var orderItem = new OrderItem(productId, "Test Product", Order.MAX_ITEM_UNITS, 100);
            order.AddItem(orderItem);

            // Act & Assert
            Assert.Throws<DomainException>(testCode: () => order.AddItem(orderItem2));
        }

        [Fact(DisplayName = "Update Unexisting Order Item")]
        [Trait("Category", "Order Tests")]
        public void UpdateOrderItem_ItemNotInTheList_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var updatedOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 5, 100);

            // Act & Assert
            Assert.Throws<DomainException>(testCode: () => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Update Valid Order Item")]
        [Trait("Category", "Order Tests")]
        public void UpdateOrderItem_ValidItem_ShouldUpdateQuantity()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            order.AddItem(orderItem);
            var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 100);
            var newQuantity = updatedOrderItem.Quantity;

            // Act
            order.UpdateItem(updatedOrderItem);

            // Assert
            Assert.Equal(expected: newQuantity, actual: order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
        }
    }
}
