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
    public class GetYearlyReminderQueryHandler
        : IRequestHandler<GetYearlyReminderQuery, List<CalenderReminderDto>>
    {

        private readonly IReminderRepository _reminderRepository;

        public GetYearlyReminderQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task<List<CalenderReminderDto>> Handle(GetYearlyReminderQuery request, CancellationToken cancellationToken)
        {
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var monthEndDate = startDate.AddMonths(1).AddDays(-1);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, monthEndDate.Day, 23, 59, 59);
            var lastDayOfMonth = endDate.Day;
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Where(c => c.Frequency == Frequency.Yearly
                    && c.StartDate.Month == request.Month
                    && c.StartDate <= endDate && (!c.EndDate.HasValue || c.EndDate >= startDate))
                 .ToListAsync();
            var reminderDto = reminders.Select(c => new CalenderReminderDto
            {
                Title = c.Subject,
                Start = new DateTime(startDate.Year, startDate.Month, c.StartDate.Day),
                End = new DateTime(startDate.Year, startDate.Month, c.StartDate.Day),
            }).ToList();

            return reminderDto;
        }
    }
}
