using POS.Common.UnitOfWork;
using POS.Data.Dto;
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
    public class DeleteInquiryCommandHandler : IRequestHandler<DeleteInquiryCommand, ServiceResponse<InquiryDto>>
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly ILogger<DeleteInquiryCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        public DeleteInquiryCommandHandler(IInquiryRepository inquiryRepository,
            ILogger<DeleteInquiryCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _inquiryRepository = inquiryRepository;
            _logger = logger;
            _uow = uow;
        }
        public async Task<ServiceResponse<InquiryDto>> Handle(DeleteInquiryCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _inquiryRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Inquiry does not exists.");
                return ServiceResponse<InquiryDto>.Return404();
            }
            entityExist.IsDeleted = true;
            _inquiryRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<InquiryDto>.Return500();
            }
            return ServiceResponse<InquiryDto>.ReturnResultWith204();
        }
    }
}
