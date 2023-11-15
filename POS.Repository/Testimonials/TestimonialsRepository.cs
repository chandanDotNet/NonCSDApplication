using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class TestimonialsRepository
        : GenericRepository<Testimonials, POSDbContext>, ITestimonialsRepository
    {
        public TestimonialsRepository(IUnitOfWork<POSDbContext> uow)
          : base(uow)
        {
        }
    }
}
