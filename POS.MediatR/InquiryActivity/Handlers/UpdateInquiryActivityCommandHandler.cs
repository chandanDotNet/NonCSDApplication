using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Entities;
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
    public class UpdateInquiryActivityCommandHandler : IRequestHandler<UpdateInquiryActivityCommand, ServiceResponse<bool>>
    {
        private readonly IInquiryActivityRepository _inquiryActivityRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInquiryActivityCommandHandler> _logger;

        public UpdateInquiryActivityCommandHandler(
            IInquiryActivityRepository inquiryActivityRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdateInquiryActivityCommandHandler> logger)
        {
            _inquiryActivityRepository = inquiryActivityRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateInquiryActivityCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _inquiryActivityRepository.FindAsync(request.Id);
            if (existingEntity == null)
            {
                _logger.LogError("Not Found");
                return ServiceResponse<bool>.Return404();
            }
            existingEntity = _mapper.Map(request, existingEntity);
            _inquiryActivityRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while adding industry");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
