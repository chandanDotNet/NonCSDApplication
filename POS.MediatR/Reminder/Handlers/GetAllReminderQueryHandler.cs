using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllReminderQueryHandler : IRequestHandler<GetAllReminderQuery, ReminderList>
    {
        private readonly IReminderRepository _reminderRepository;
        public GetAllReminderQueryHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }
        public async Task<ReminderList> Handle(GetAllReminderQuery request, CancellationToken cancellationToken)
        {
            return await _reminderRepository.GetReminders(request.ReminderResource);
        }
    }
}
