using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class WarehouseRepository 
        : GenericRepository<Warehouse, POSDbContext>, IWarehouseRepository
    {
        public WarehouseRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
