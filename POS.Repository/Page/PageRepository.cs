using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class PageRepository : GenericRepository<Page, POSDbContext>,
          IPageRepository
    {
        public PageRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}
