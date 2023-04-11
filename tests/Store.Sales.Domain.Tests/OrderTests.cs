using Store.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace Store.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add New Item Order")]
        [Trait("Category", "Sales - Order")]
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
        [Trait("Category", "Sales - Order")]
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
        [Trait("Category", "Sales - Order")]
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
        [Trait("Category", "Sales - Order")]
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
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_ItemNotInTheList_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var updatedOrderItem = new OrderItem(Guid.NewGuid(), "Test Product", 5, 100);

            // Act & Assert
            Assert.Throws<DomainException>(testCode: () => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Update Valid Order Item")]
        [Trait("Category", "Sales - Order")]
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

        [Fact(DisplayName = "Update Order Item Validate Total Amount")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_OrderWithDifferentProducts_ShouldUpdateTotalAmount()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var existingOrder1 = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);
            var existingOrder2 = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(existingOrder1);
            order.AddItem(existingOrder2);

            var updatedOrderItem = new OrderItem(productId, "Test Product", 5, 15);
            var totalOrder = existingOrder1.Quantity * existingOrder1.UnitPrice +
                               updatedOrderItem.Quantity * updatedOrderItem.UnitPrice;

            // Act
            order.UpdateItem(updatedOrderItem);

            // Assert
            Assert.Equal(expected: totalOrder, actual: order.TotalAmount);
        }

        [Fact(DisplayName = "Update Order Item Validate Quantity Above The Allowed")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_ItemUnitsAboveAllowed_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var existingOrder = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(existingOrder);

            var updatedOrderItem = new OrderItem(productId, "Test Product", Order.MAX_ITEM_UNITS + 1, 15);

            // Act & Assert
            Assert.Throws<DomainException>(testCode: () => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Remove Unexisting Order Item")]
        [Trait("Category", "Sales - Order")]
        public void RemoveOrderItem_ItemNotInTheList_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItemToRemove = new OrderItem(Guid.NewGuid(), "Test Product", 5, 15);

            // Act & Assert
            Assert.Throws<DomainException>(testCode: () => order.RemoveItem(orderItemToRemove));
        }

        [Fact(DisplayName = "Remove Order Item Should Update Order Total Amount")]
        [Trait("Category", "Sales - Order")]
        public void RemoveOrderItem_ExistingItem_ShouldUpdateTotalAmount()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(Guid.NewGuid(), "XPTO Product", 2, 100);
            var orderItem2 = new OrderItem(productId, "Test Product", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var totalAmount = orderItem2.Quantity * orderItem2.UnitPrice;

            // Act
            order.RemoveItem(orderItem1);

            // Assert
            Assert.Equal(expected: totalAmount, actual: order.TotalAmount);
        }

        [Fact(DisplayName = "Apply valid voucher")]
        [Trait("Category", "Sales - Order")]
        public void Order_ApplyValidVoucher_ShouldCompleteWithSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var voucher = new Voucher(code: "PROMO-15-MZN", discountAmount: 15, discountPercent: null, DiscountType.Amount, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);

            // Act
            var result = order.ApplyVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Apply invalid voucher")]
        [Trait("Category", "Sales - Order")]
        public void Order_ApplyInValidVoucher_ShouldCompleteWithSuccess()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var voucher = new Voucher(code: "PROMO-15-MZN", discountAmount: 15, discountPercent: null, DiscountType.Amount, quantity: 1, DateTime.Now.AddDays(-1), isActive: true, isUsed: true);

            // Act
            var result = order.ApplyVoucher(voucher);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Apply voucher type discount amount")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_VoucherTypeDiscountAmount_ShouldDiscountTotalAmount()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(Guid.NewGuid(), "XPTO Product", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var voucher = new Voucher(code: "PROMO-15-MZN", discountAmount: 15, discountPercent: null, DiscountType.Amount, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);

            var totalWithDiscountAmount = order.TotalAmount - voucher.DiscountAmount;

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            Assert.Equal(expected: totalWithDiscountAmount, actual: order.TotalAmount);
        }

        [Fact(DisplayName = "Apply voucher type discount percentage")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_VoucherTypeDiscountPercentage_ShouldDiscountTotalAmount()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(Guid.NewGuid(), "XPTO Product", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var voucher = new Voucher(code: "PROMO-15-OFF", discountAmount: null, discountPercent: 15, DiscountType.Percentage, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);

            var discount = (order.TotalAmount * voucher.DiscountPercent) / 100;
            var totalWithDiscountAmount = order.TotalAmount - discount;

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            Assert.Equal(expected: totalWithDiscountAmount, actual: order.TotalAmount);
        }

        [Fact(DisplayName = "Apply voucher discount exceeds total amount")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_DiscountExceedsOrderTotalAmount_OrderShouldHaveZeroValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "XPTO Product", 2, 100);
            var voucher = new Voucher(code: "PROMO-15-OFF", discountAmount: 300, discountPercent: null, DiscountType.Amount, quantity: 1, DateTime.Now.AddDays(15), isActive: true, isUsed: false);

            // Act
            order.ApplyVoucher(voucher);

            // Assert
            Assert.Equal(expected: 0, actual: order.TotalAmount);
        }
    }
}
