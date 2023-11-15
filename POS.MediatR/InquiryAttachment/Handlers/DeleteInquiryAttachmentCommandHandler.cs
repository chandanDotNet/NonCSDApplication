using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DeleteInquiryAttachmentCommandHandler : IRequestHandler<DeleteInquiryAttachmentCommand, ServiceResponse<bool>>
    {
        private readonly IInquiryAttachmentRepository _inquiryAttachmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteInquiryAttachmentCommandHandler> _logger;

        public DeleteInquiryAttachmentCommandHandler(
            IInquiryAttachmentRepository inquiryAttachmentRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteInquiryAttachmentCommandHandler> logger
          )
        {
            _inquiryAttachmentRepository = inquiryAttachmentRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteInquiryAttachmentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _inquiryAttachmentRepository.FindAsync(request.Id);
            if (entity != null)
            {
                _inquiryAttachmentRepository.Delete(entity);
                if (await _uow.SaveAsync() <= 0)
                {
                    _logger.LogError("Delete a Inquiry Attachment failed on delete.", request);
                    return ServiceResponse<bool>.ReturnFailed(500, $"Creating a Inquiry Attachment  failed on save.");
                }
                _logger.LogError("Delete a Inquiry Attachment failed on delete.", request);
                return ServiceResponse<bool>.ReturnResultWith200(true);
            }
            _logger.LogError("Delete a Inquiry Attachment failed on delete.", request);
            return ServiceResponse<bool>.ReturnFailed(404, $"Inquiry Attachement is not found.");
        }
    }
}
