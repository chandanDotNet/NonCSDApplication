using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllReminderSchedulerQueryHandler
         : IRequestHandler<GetAllReminderSchedulerQuery, List<ReminderSchedulerDto>>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        public GetAllReminderSchedulerQueryHandler(IReminderSchedulerRepository reminderSchedulerRepository)
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }

        public async Task<List<ReminderSchedulerDto>> Handle(GetAllReminderSchedulerQuery request, CancellationToken cancellationToken)
        {
            var schedulers = await _reminderSchedulerRepository
                .All
                .Where(c => c.ReferenceId == request.ReferenceId && c.Application == request.Application)
                .Select(cs => new ReminderSchedulerDto
                {
                    Application = cs.Application,
                    CreatedDate = cs.CreatedDate,
                    Duration = cs.Duration,
                    Message = cs.Message,
                    ReferenceId = cs.ReferenceId,
                    Subject = cs.Subject,
                    UserName = $"{cs.User.FirstName} {cs.User.LastName}"
                }).ToListAsync();
            return schedulers;
        }
    }
}
