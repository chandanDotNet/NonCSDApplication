using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data.Entities;
using POS.Domain;

namespace POS.Repository
{
    public class InquiryAttachmentRepository : GenericRepository<InquiryAttachment, POSDbContext>, IInquiryAttachmentRepository
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        public InquiryAttachmentRepository(IUnitOfWork<POSDbContext> uow) : base(uow)
        {

        }
    }
}
