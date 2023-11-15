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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DeleteInquirySourceCommandHandler 
        : IRequestHandler<DeleteInquirySourceCommand, ServiceResponse<bool>>
    {
        private readonly IInquirySourceRepository _inquirySourceRepository;
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteInquirySourceCommandHandler> _logger;
        public DeleteInquirySourceCommandHandler(
           IInquirySourceRepository inquirySourceRepository,
            IInquiryRepository inquiryRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteInquirySourceCommandHandler> logger
            )
        {
            _inquirySourceRepository = inquirySourceRepository;
            _inquiryRepository = inquiryRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteInquirySourceCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _inquirySourceRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                _logger.LogError("Inquiry Source does not Exist");
                return ServiceResponse<bool>.Return409("Inquiry Source does not  Exist.");
            }

            var exitingInquiry = _inquiryRepository.AllIncluding(c => c.InquirySource).Any(c => c.InquirySource.Id == existingEntity.Id);
            if (exitingInquiry)
            {
                _logger.LogError("Inquiry Source can not be Deleted because it is use in Inquiry");
                return ServiceResponse<bool>.Return409("Inquiry Source can not be Deleted because it is use in Inquiry.");
            }

            _inquirySourceRepository.Delete(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Inquiry Source.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
