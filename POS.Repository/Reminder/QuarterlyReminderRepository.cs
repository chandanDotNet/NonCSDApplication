using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class QuarterlyReminderRepository : GenericRepository<QuarterlyReminder, POSDbContext>,
    IQuarterlyReminderRepository
    {
        public QuarterlyReminderRepository(
            IUnitOfWork<POSDbContext> uow) : base(uow)
        {
        }
    }
}
