using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class InquiryProductRepository : GenericRepository<InquiryProduct, POSDbContext>, IInquiryProductRepository
    {
        public InquiryProductRepository(IUnitOfWork<POSDbContext> uow) : base(uow)
        {
        }
    }
}
