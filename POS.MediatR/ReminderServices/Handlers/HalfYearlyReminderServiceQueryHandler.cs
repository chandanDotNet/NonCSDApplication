using POS.Common.UnitOfWork;
using POS.Data.Entities;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class HalfYearlyReminderServiceQueryHandler : IRequestHandler<HalfYearlyReminderServiceQuery, bool>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;

        public HalfYearlyReminderServiceQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository,
            IUnitOfWork<POSDbContext> uow)
        {
            _reminderRepository = reminderRepository;
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }
        public async Task<bool> Handle(HalfYearlyReminderServiceQuery request, CancellationToken cancellationToken)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToUniversalTime();
            var reminders = await _reminderRepository.All
                   .Include(c => c.ReminderUsers)
                   .Where(c => c.Frequency == Frequency.HalfYearly
            && c.StartDate <= currentDate && (!c.EndDate.HasValue || c.EndDate >= currentDate)
            && c.HalfYearlyReminders.Any(qr => qr.Day == currentDate.Day && qr.Month == currentDate.Month)
             )
            .ToListAsync();

            if (reminders != null && reminders.Count() > 0)
            {
                return await _reminderSchedulerRepository.AddMultiReminder(reminders);
            }
            return true;


        }
    }
}
