using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
    public class HalfYearlyReminderRepository : GenericRepository<HalfYearlyReminder, POSDbContext>,
        IHalfYearlyReminderRepository
    {
        public HalfYearlyReminderRepository(
            IUnitOfWork<POSDbContext> uow) : base(uow)
        {
        }
    }
}
