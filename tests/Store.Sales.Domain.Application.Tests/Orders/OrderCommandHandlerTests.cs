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
        public async Task AddOrderItem_NewOrder_ShouldExecuteWithSuccess()
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

        [Fact(DisplayName = "Add New Drarft Order Item with success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task AddOrderItem_NewItemToTheDraftOrder_ShouldExecuteWithSuccess()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var order = Order.OrderFactory.NewDraftOrder(customerId);
            var existingOrderItem = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
            order.AddItem(existingOrderItem);

            var orderCommand = new AddOrderItemCommand(customerId, Guid.NewGuid(), "Test Product", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(r => r.GetOrderDraftByCustomerId(customerId)).Returns(Task.FromResult(order));
            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result); 
            mocker.GetMock<IOrderRepository>().Verify(expression: r => r.AddItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(expression: r => r.Update(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Existing Item to the Draft Order with success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task AddOrderItem_ExistingItemToTheDraftOrder_ShouldExecuteWithSuccess()
        {
            var customerId = Guid.NewGuid(); 
            var productId = Guid.NewGuid();

            var order = Order.OrderFactory.NewDraftOrder(customerId);
            var existingOrderItem = new OrderItem(productId, "Product Xpto", 2, 100);
            order.AddItem(existingOrderItem);

            var orderCommand = new AddOrderItemCommand(customerId, productId, "Product Xpto", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(r => r.GetOrderDraftByCustomerId(customerId)).Returns(Task.FromResult(order));
            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(expression: r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(expression: r => r.Update(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Item Invalid Command")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task AddOrderItem_InvalidCommand_ShouldReturnFalseAndDispatchANotificationEvent()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(customerId: Guid.Empty, productId: Guid.Empty, name:"", quantity: 0, unitPrice: 0);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            mocker.GetMock<IMediator>().Verify(expression: r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
