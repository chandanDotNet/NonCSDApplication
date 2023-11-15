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

    public class GetOneTimeReminerQueryHandler
        : IRequestHandler<GetOneTimeReminerQuery, List<CalenderReminderDto>>
    {
        private readonly IReminderRepository _reminderRepository;

        public GetOneTimeReminerQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        public async Task<List<CalenderReminderDto>> Handle(GetOneTimeReminerQuery request, CancellationToken cancellationToken)
        {
            var result = new List<CalenderReminderDto>();
            var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 1);
            var monthEndDate = startDate.AddMonths(1).AddDays(-1);
            var endDate = new DateTime(monthEndDate.Year, monthEndDate.Month, monthEndDate.Day, 23, 59, 59);
            var reminders = await _reminderRepository.All
                 .Include(c => c.ReminderUsers)
                 .Include(c => c.DailyReminders)
                 .Where(c => c.Frequency == Frequency.OneTime
                    && c.StartDate <= endDate && (!c.EndDate.HasValue || c.EndDate >= startDate))
                 .ToListAsync();

            return reminders.Select(d => new CalenderReminderDto
            {
                Start = d.StartDate,
                End = d.StartDate,
                Title = d.Subject
            }).ToList();
        }
    }
}
