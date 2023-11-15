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
    public class DeleteInquiryActivityCommandHandler : IRequestHandler<DeleteInquiryActivityCommand, ServiceResponse<bool>>
    {
        private readonly IInquiryActivityRepository _inquiryActivityRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteInquiryActivityCommandHandler> _logger;

        public DeleteInquiryActivityCommandHandler(
            IInquiryActivityRepository inquiryActivityRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteInquiryActivityCommandHandler> logger)
        {
            _inquiryActivityRepository = inquiryActivityRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteInquiryActivityCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _inquiryActivityRepository.FindAsync(request.Id);
            if (existingEntity == null)
            {
                _logger.LogError("Not Found");
                return ServiceResponse<bool>.Return404();
            }
            _inquiryActivityRepository.Remove(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while adding industry");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
