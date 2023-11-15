using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class ReminderSchedulerServiceQueryHandler : IRequestHandler<ReminderSchedulerServiceQuery, bool>
    {
        private readonly IReminderSchedulerRepository _reminderSchedulerRepository;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly ILogger<ReminderSchedulerServiceQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IHubContext<UserHub, IHubClient> _hubContext;

        public ReminderSchedulerServiceQueryHandler(IReminderRepository reminderRepository,
            IReminderSchedulerRepository reminderSchedulerRepository,
            IEmailSMTPSettingRepository emailSMTPSettingRepository,
            IUnitOfWork<POSDbContext> uow,
             ILogger<ReminderSchedulerServiceQueryHandler> logger,
             PathHelper pathHelper,
             IConnectionMappingRepository connectionMappingRepository,
             IHubContext<UserHub, IHubClient> hubContext)
        {
            _reminderSchedulerRepository = reminderSchedulerRepository;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _logger = logger;
            _pathHelper = pathHelper;
            _uow = uow;
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(ReminderSchedulerServiceQuery request, CancellationToken cancellationToken)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToUniversalTime();

            var reminderSchedulers = await _reminderSchedulerRepository.All
                                            .OrderBy(c => c.Duration)
                                            .Where(c => c.IsActive && c.Duration <= currentDate)
                                            .Take(10)
                                            .ToListAsync();

            if (reminderSchedulers.Count() > 0)
            {
                var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();

                foreach (var reminderScheduler in reminderSchedulers)
                {
                    await _hubContext.Clients.All.SendNotification(reminderScheduler.UserId);
                    if (reminderScheduler.IsEmailNotification)
                    {
                        if (defaultSmtp != null)
                        {
                            try
                            {
                                EmailHelper.SendEmail(new SendEmailSpecification
                                {
                                    Body = reminderScheduler.Message,
                                    FromAddress = _pathHelper.ReminderFromEmail,
                                    Host = defaultSmtp.Host,
                                    IsEnableSSL = defaultSmtp.IsEnableSSL,
                                    Password = defaultSmtp.Password,
                                    Port = defaultSmtp.Port,
                                    Subject = reminderScheduler.Subject,
                                    ToAddress = reminderScheduler.User.Email,
                                    CCAddress = "",
                                    UserName = defaultSmtp.UserName,
                                    Attechments = new List<FileInfo>()
                                });
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message, ex);
                            }
                        }
                    }
                    reminderScheduler.IsActive = false;
                }
                _reminderSchedulerRepository.UpdateRange(reminderSchedulers);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error while Save Reminder Schedulers");
                    return false;
                }
            }
            return true;
        }
    }
}
