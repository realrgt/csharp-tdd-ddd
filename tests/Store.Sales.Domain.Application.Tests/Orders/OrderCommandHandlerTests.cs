using MediatR;
using Moq;
using Moq.AutoMock;
using Store.Sales.Application.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Store.Sales.Domain.Application.Tests.Orders
{
    public class OrderCommandHandlerTests
    {
        [Fact(DisplayName = "Add Item New Order with success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task AddOrderItemCommand_CommandIsValid_ShouldPassInValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(expression: r => r.Add(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(expression: r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
    }
}
