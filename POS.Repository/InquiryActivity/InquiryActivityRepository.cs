using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Entities;
using POS.Domain;

namespace POS.Repository
{
    public class InquiryActivityRepository : GenericRepository<InquiryActivity, POSDbContext>, IInquiryActivityRepository
    {
        public InquiryActivityRepository(IUnitOfWork<POSDbContext> uow) : base(uow)
        {

        }
    }
}
