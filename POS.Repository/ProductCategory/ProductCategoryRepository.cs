using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory, POSDbContext>, IProductCategoryRepository
    {
        public ProductCategoryRepository(IUnitOfWork<POSDbContext> uow)
            : base(uow)
        {
        }
    }
}
