using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class CounterRepository : GenericRepository<Counter, POSDbContext>, ICounterRepository
    {
        public CounterRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
