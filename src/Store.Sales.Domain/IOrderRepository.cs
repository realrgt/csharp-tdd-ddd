using Store.Core.Data;
using System;
using System.Threading.Tasks;

namespace Store.Sales.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Update(Order order);
        Task<Order> GetOrderDraftByCustomerId(Guid customerId);
        void AddItem(OrderItem orderItem);
    }
}
