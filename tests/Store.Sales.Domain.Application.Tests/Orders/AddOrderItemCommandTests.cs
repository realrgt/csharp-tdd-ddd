using Store.Sales.Application.Commands;
using System;
using System.Linq;
using Xunit;

namespace Store.Sales.Domain.Application.Tests.Orders
{
    public class AddOrderItemCommandTests
    {
        [Fact(DisplayName = "Add Item Command Valid")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsValid_ShouldPassInValidation() 
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 2, 100);

            // Act
            var result = orderCommand.IsValid();

            // Asser
            Assert.True(result);
        }

        [Fact(DisplayName = "Add Item Command Invalid")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsInvalid_ShouldPassInValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.IdClientErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.IdProductErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.NameErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.QuantityMinErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.PriceErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Add Item Command units above the allowed")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_QuantityUnitsAboveAllowed_ShouldPassInValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", Order.MAX_ITEM_UNITS + 1, 100);

            // Act
            var result = orderCommand.IsValid();

            // Asser
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.QuantityMaxErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
