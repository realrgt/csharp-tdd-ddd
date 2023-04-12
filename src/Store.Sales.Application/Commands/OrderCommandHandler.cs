using MediatR;
using Store.Sales.Application.Events;
using Store.Sales.Domain;

namespace Store.Sales.Application.Commands
{
    public class OrderCommandHandler 
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public bool Handle(AddOrderItemCommand message)
        {
            _orderRepository.Add(Order.OrderFactory.NewDraftOrder(message.ClientId));
            _mediator.Publish(new OrderItemAddedEvent());

            return true;
        }
    }
}
