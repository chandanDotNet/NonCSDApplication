using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Reminder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Reminder.Handlers
{
    internal class GetReminderNotificationQueryHandler : IRequestHandler<GetReminderNotificationQuery, ReminderList>
    {
        private readonly IReminderRepository _reminderRepository;
        public GetReminderNotificationQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        public async Task<ReminderList> Handle(GetReminderNotificationQuery request, CancellationToken cancellationToken)
        {
            return await _reminderRepository.GetReminders(request.ReminderResource);
        }
    }
}