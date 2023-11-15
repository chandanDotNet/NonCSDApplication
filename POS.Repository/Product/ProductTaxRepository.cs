using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class ProductTaxRepository 
        : GenericRepository<ProductTax, POSDbContext>, IProductTaxRepository
    {
        public ProductTaxRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
