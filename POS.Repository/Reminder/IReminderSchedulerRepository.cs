using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Entities;
using POS.Data.Resources;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
   public interface IReminderSchedulerRepository : IGenericRepository<ReminderScheduler>
    {
        Task<bool> AddMultiReminder(List<Reminder> reminders);
        Task<PagedList<ReminderScheduler>> GetReminders(ReminderResource reminderResource);
        Task<bool> MarkAsRead();
    }
}
