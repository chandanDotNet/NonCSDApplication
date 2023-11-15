using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class CountryRepository : GenericRepository<Country, POSDbContext>, ICountryRepository
    {
        public CountryRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
