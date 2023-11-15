using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class DailyReminderRepository : GenericRepository<DailyReminder, POSDbContext>,
        IDailyReminderRepository
    {
        public DailyReminderRepository(
            IUnitOfWork<POSDbContext> uow) : base(uow)
        {
        }
    }
}
