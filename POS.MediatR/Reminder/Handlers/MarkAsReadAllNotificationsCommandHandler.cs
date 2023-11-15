using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class MarkAsReadAllNotificationsCommandHandler : IRequestHandler<MarkAsReadAllNotificationsCommand, bool>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly UserInfoToken _userInfoToken;
        private readonly IHubContext<UserHub, IHubClient> _hubContext;

        public MarkAsReadAllNotificationsCommandHandler(
            IReminderSchedulerRepository reminderSchedulerRepository,
            UserInfoToken userInfoToken,
            IHubContext<UserHub, IHubClient> hubContext)
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _userInfoToken = userInfoToken;
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(MarkAsReadAllNotificationsCommand request, CancellationToken cancellationToken)
        {
            var flag = await _reminderSchedulerRepository.MarkAsRead();
            await _hubContext.Clients.All.SendNotification(Guid.Parse(_userInfoToken.Id));
            return flag;
        }
    }
}
