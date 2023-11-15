using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class SendEmailSchedulerSupplierCommandHandler : IRequestHandler<SendEmailSchedulerSupplierCommand, bool>
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<SendEmailSchedulerSupplierCommandHandler> _logger;
        private readonly IEmailSMTPSettingRepository _emailSMTPSettingRepository;
        private readonly PathHelper _pathHelper;

        public SendEmailSchedulerSupplierCommandHandler(
            ISendEmailRepository sendEmailRepository,
            ILogger<SendEmailSchedulerSupplierCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IEmailSMTPSettingRepository emailSMTPSettingRepository,
            PathHelper pathHelper
            )
        {
            _sendEmailRepository = sendEmailRepository;
            _uow = uow;
            _logger = logger;
            _emailSMTPSettingRepository = emailSMTPSettingRepository;
            _pathHelper = pathHelper;
        }

        public async Task<bool> Handle(SendEmailSchedulerSupplierCommand request, CancellationToken cancellationToken)
        {
            //try
            //{
            //    var sendEmails = await _sendEmailRepository.All
            //        .Include(c => c.Supplier)
            //        .ThenInclude(c => c.SupplierEmails)
            //        .OrderByDescending(c => c.CreatedDate)
            //        .Where(c => !c.IsSend)
            //        .Take(10)
            //        .ToListAsync();
            //    if (sendEmails.Count > 0)
            //    {
            //        var defaultSmtp = await _emailSMTPSettingRepository.FindBy(c => c.IsDefault).FirstOrDefaultAsync();
            //        foreach (var sendEmail in sendEmails)
            //        {
            //            if (defaultSmtp != null)
            //            {
            //                if (!string.IsNullOrEmpty(sendEmail.Supplier.SupplierEmails[0].Email))
            //                {
            //                    try
            //                    {
            //                        EmailHelper.SendEmail(new SendEmailSpecification
            //                        {
            //                            Body = sendEmail.Message,
            //                            FromAddress = _pathHelper.ReminderFromEmail,
            //                            Host = defaultSmtp.Host,
            //                            IsEnableSSL = defaultSmtp.IsEnableSSL,
            //                            Password = defaultSmtp.Password,
            //                            Port = defaultSmtp.Port,
            //                            Subject = sendEmail.Subject,
            //                            ToAddress = sendEmail.Supplier.SupplierEmails[0].Email,
            //                            CCAddress = "",
            //                            UserName = defaultSmtp.UserName,
            //                            Attechments = new List<FileInfo>()
            //                        });
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        _logger.LogError(ex.Message, ex);
            //                    }
            //                }
            //            }
            //            sendEmail.IsSend = true;
            //            sendEmail.Supplier = null;
            //        }
            //        _sendEmailRepository.UpdateRange(sendEmails);
            //        if (await _uow.SaveAsync() <= 0)
            //        {
            //            return false;
            //        }
            //        return true;
            //    }
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message, ex);
            //    return true;
            //}
            return true;
        }
    }
}
