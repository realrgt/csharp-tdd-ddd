using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Sales.Domain
{
    public interface IOrderRepository
    {
        void Add(Order order);
    }
}
