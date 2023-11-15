using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class TaxRepository : GenericRepository<Tax, POSDbContext>, ITaxRepository
    {
        public TaxRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
