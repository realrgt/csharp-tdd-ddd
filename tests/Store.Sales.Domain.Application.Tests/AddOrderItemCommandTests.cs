using Store.Sales.Application.Commands;
using System;
using Xunit;

namespace Store.Sales.Domain.Application.Tests
{
    public class AddOrderItemCommandTests
    {
        [Fact(DisplayName = "Add Item Command Valid")]
        [Trait("Category", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsEmpty_ShouldPassInValidation() 
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 2, 100);

            // Act
            var result = orderCommand.IsValid();

            // Asser
            Assert.True(result);
        }
    }
}
