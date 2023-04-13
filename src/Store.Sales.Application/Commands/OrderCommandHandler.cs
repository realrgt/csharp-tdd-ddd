using MediatR;
using Store.Core.DomainObjects;
using Store.Sales.Application.Events;
using Store.Sales.Domain;
using System.Linq;
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
            if (!message.IsValid())
            {
                foreach (var error in message.ValidationResult.Errors)
                {
                    await _mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage), cancellationToken);
                }

                return false;
            }

            var order = await _orderRepository.GetOrderDraftByCustomerId(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnitPrice);

            if (order == null) {
                order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
                order.AddItem(orderItem);

                _orderRepository.Add(order);
            }
            else {

                var existingOrderItem = order.IsExistingOrderItem(orderItem);
                order.AddItem(orderItem);

                if (existingOrderItem)
                {
                    _orderRepository.UpdateItem(order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId));
                } else
                {
                    _orderRepository.AddItem(orderItem);
                }

                _orderRepository.Update(order);
            }

            order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.Name, message.UnitPrice, message.Quantity));

            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
