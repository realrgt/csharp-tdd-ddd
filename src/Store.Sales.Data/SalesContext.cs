using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.Core.Data;
using System.Threading.Tasks;

namespace Store.Sales.Data
{
    public class SalesContext : DbContext, IUnitOfWork
    {

        private readonly IMediator _mediator;

        public SalesContext(DbContextOptions<SalesContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediator.PublishEvents(this);

            return success;
        }
    }
}
