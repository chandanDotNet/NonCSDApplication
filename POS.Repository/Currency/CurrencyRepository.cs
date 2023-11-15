using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class CurrencyRepository 
        : GenericRepository<Currency, POSDbContext>, ICurrencyRepository
    {
        public CurrencyRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
