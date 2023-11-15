using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetDailyReminderQueryHandler
        : IRequestHandler<GetDailyReminderQuery, List<CalenderReminderDto>>
    {
        private readonly IReminderRepository _reminderRepository;

        public GetDailyReminderQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        public async Task<List<CalenderReminderDto>> Handle(GetDailyReminderQuery request, CancellationToken cancellationToken)
        {
            var result = new List<CalenderReminderDto>();
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var monthEndDate = startDate.AddMonths(1).AddDays(-1);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, monthEndDate.Day, 23, 59, 59);
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Include(c => c.DailyReminders)
                 .Where(c => c.Frequency == Frequency.Daily
                    && c.StartDate <= endDate && (!c.EndDate.HasValue || c.EndDate >= startDate))
                 .ToListAsync();

            reminders.ForEach(re =>
            {
                var reminderStartDate = startDate <= re.StartDate ? re.StartDate : startDate;
                var reminderEndDate = re.EndDate.HasValue && endDate >= re.EndDate ? re.EndDate.Value : endDate;
                var dailyReminders = Enumerable.Range(0, 1 + reminderEndDate.Subtract(reminderStartDate).Days)
                    .Where(d => re.DailyReminders.Any(dr => dr.IsActive && dr.DayOfWeek == reminderStartDate.AddDays(d).DayOfWeek))
                 .Select(offset => reminderStartDate.AddDays(offset))
                 .ToList();
                result.AddRange(dailyReminders.Select(d => new CalenderReminderDto
                {
                    Start = d,
                    End = d,
                    Title = re.Subject
                }));
            });
            return result;
        }
    }
}
