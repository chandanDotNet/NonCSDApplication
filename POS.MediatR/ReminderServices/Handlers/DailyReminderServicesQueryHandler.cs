using POS.Common.UnitOfWork;
using POS.Data.Entities;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DailyReminderServicesQueryHandler : IRequestHandler<DailyReminderServicesQuery, bool>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;

        public DailyReminderServicesQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository
            )
        {
            _reminderRepository = reminderRepository;
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }
        public async Task<bool> Handle(DailyReminderServicesQuery request, CancellationToken cancellationToken)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToUniversalTime();
            var dayOfWeek = currentDate.DayOfWeek;

            var reminders = await _reminderRepository
                .All
                .Include(c => c.ReminderUsers)
                .Where(c => c.Frequency == Frequency.Daily && c.IsRepeated
            && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
            && c.DailyReminders.Any(dr => dr.DayOfWeek == dayOfWeek && dr.IsActive)).ToListAsync();
          
            if (reminders.Count() > 0)
            {
                return await _reminderSchedulerRepository.AddMultiReminder(reminders);
            }
            return true;
        }
    }
}
