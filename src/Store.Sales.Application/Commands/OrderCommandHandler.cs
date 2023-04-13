using MediatR;
using Store.Sales.Application.Events;
using Store.Sales.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Store.Sales.Application.Commands
{
    public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnitPrice);
            var order = Order.OrderFactory.NewDraftOrder(message.ClientId);
            order.AddItem(orderItem);

            _orderRepository.Add(order);

            order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.Name, message.UnitPrice, message.Quantity));

            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
