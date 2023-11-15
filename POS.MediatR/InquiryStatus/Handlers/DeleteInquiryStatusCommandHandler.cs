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
    public class DeleteInquiryStatusCommandHandler : IRequestHandler<DeleteInquiryStatusCommand, ServiceResponse<bool>>
    {
        private readonly IInquiryStatusRepository _inquiryStatusRepository;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteInquiryStatusCommandHandler> _logger;

        public DeleteInquiryStatusCommandHandler(
           IInquiryStatusRepository inquiryStatusRepository,
           IInquiryRepository inquiryRepository,
        IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteInquiryStatusCommandHandler> logger
            )
        {
            _inquiryStatusRepository = inquiryStatusRepository;
            _inquiryRepository = inquiryRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteInquiryStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _inquiryStatusRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Inquiry Status Does not exists");
                return ServiceResponse<bool>.Return404("Inquiry Status Does not exists");
            }

            var exitingInquiry = _inquiryRepository.AllIncluding(c => c.InquiryStatus).Any(c => c.InquiryStatus.Id == entityExist.Id);
            if (exitingInquiry)
            {
                _logger.LogError("Inquiry Status can not be Deleted because it is use in Inquiry");
                return ServiceResponse<bool>.Return409("Inquiry Status can not be Deleted because it is use in Inquiry.");
            }

            _inquiryStatusRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Inquiry Status.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
