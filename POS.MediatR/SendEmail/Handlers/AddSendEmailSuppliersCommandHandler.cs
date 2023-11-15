using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddSendEmailSuppliersCommandHandler : IRequestHandler<AddSendEmailSuppliersCommand, bool>
    {
        private readonly ISendEmailRepository _sendEmailRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddSendEmailSuppliersCommandHandler> _logger;

        public AddSendEmailSuppliersCommandHandler(
            ISendEmailRepository sendEmailRepository,
            ILogger<AddSendEmailSuppliersCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow
            )
        {
            _sendEmailRepository = sendEmailRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<bool> Handle(AddSendEmailSuppliersCommand request, CancellationToken cancellationToken)
        {
            List<SendEmail> lstSendEmail = new List<SendEmail>();
            if (request.Suppliers.Count > 0)
            {
                foreach (var supplier in request.Suppliers)
                {
                    var sendEmail = new SendEmail
                    {
                        Id = Guid.NewGuid(),
                        Subject = request.Subject,
                        Message = request.Message,
                        IsSend = false,
                        SupplierId = supplier
                    };
                    lstSendEmail.Add(sendEmail);
                }
                _sendEmailRepository.AddRange(lstSendEmail);

                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Error to Save Send Email");
                    return false;
                }
            }
            return true;
        }
    }
}
