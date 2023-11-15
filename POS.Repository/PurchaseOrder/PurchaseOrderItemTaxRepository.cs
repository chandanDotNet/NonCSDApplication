using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class PurchaseOrderItemTaxRepository 
        : GenericRepository<PurchaseOrderItemTax, POSDbContext>, IPurchaseOrderItemTaxRepository
    {
        public PurchaseOrderItemTaxRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
