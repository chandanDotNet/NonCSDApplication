using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class EmailTemplateRepository : GenericRepository<EmailTemplate, POSDbContext>,
          IEmailTemplateRepository
    {
        public EmailTemplateRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {

        }
    }
}

