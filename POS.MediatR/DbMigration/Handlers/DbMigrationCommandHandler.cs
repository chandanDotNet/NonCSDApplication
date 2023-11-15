using POS.Common.UnitOfWork;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DbMigrationCommandHandler : IRequestHandler<DbMigrationCommand, bool>
    {

        private readonly IUnitOfWork<POSDbContext> _uow;
        public DbMigrationCommandHandler(
            IUnitOfWork<POSDbContext> uow
            )
        {
            _uow = uow;
        }
        public async Task<bool> Handle(DbMigrationCommand request, CancellationToken cancellationToken)
        {
            await _uow.Context.Database.MigrateAsync();
            return true;
        }
    }
}
