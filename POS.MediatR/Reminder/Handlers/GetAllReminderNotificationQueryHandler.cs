using POS.Data.Entities;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllReminderNotificationQueryHandler : IRequestHandler<GetAllReminderNotificationQuery, PagedList<ReminderScheduler>>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;

        public GetAllReminderNotificationQueryHandler(
            IReminderSchedulerRepository reminderSchedulerRepository
           )
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
        }

        public async Task<PagedList<ReminderScheduler>> Handle(GetAllReminderNotificationQuery request, CancellationToken cancellationToken)
        {
            return await _reminderSchedulerRepository.GetReminders(request.ReminderResource);
        }
    }
}
