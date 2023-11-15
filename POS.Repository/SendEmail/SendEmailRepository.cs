using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
   public class SendEmailRepository : GenericRepository<SendEmail, POSDbContext>,
          ISendEmailRepository
    {
        public SendEmailRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}
