using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddReminderSchedulerCommandHandler
        : IRequestHandler<AddReminderSchedulerCommand, ServiceResponse<bool>>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<AddReminderSchedulerCommandHandler> _logger;

        public AddReminderSchedulerCommandHandler(
            IReminderSchedulerRepository reminderSchedulerRepository,

            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<AddReminderSchedulerCommandHandler> logger)
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(AddReminderSchedulerCommand request, CancellationToken cancellationToken)
        {
            if (!request.UserIds.Any())
            {
                request.UserIds.Add(Guid.Parse(_userInfoToken.Id));
            }

            request.UserIds.ForEach(userId =>
            {
                _reminderSchedulerRepository.Add(new Data.Entities.ReminderScheduler
                {
                    Application = request.Application,
                    CreatedDate = DateTime.UtcNow,
                    Duration = request.CreatedDate,
                    Frequency = Data.Entities.Frequency.OneTime,
                    IsActive = true,
                    IsEmailNotification = request.IsEmailNotification,
                    IsRead = false,
                    Message = request.Message,
                    ReferenceId = request.ReferenceId,
                    Subject = request.Subject,
                    UserId = userId
                });
            });

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Reminder");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnResultWith201(true);
        }
    }
}
