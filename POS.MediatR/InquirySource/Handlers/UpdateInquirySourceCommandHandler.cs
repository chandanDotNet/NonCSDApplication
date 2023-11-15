using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
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
    public class UpdateInquirySourceCommandHandler
        : IRequestHandler<UpdateInquirySourceCommand, ServiceResponse<bool>>
    {
        private readonly IInquirySourceRepository _inquirySourceRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateInquirySourceCommandHandler> _logger;
        private readonly IMapper _mapper;
        public UpdateInquirySourceCommandHandler(
           IInquirySourceRepository inquirySourceRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateInquirySourceCommandHandler> logger,
            IMapper mapper
            )
        {
            _inquirySourceRepository = inquirySourceRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<bool>> Handle(UpdateInquirySourceCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _inquirySourceRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Inquiry Source Already Exist for another Delivery Method.");
                return ServiceResponse<bool>.Return409("Inquiry Source Already Exist for another Delivery Method.");
            }
            existingEntity = await _inquirySourceRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            existingEntity.Name = request.Name;
            _inquirySourceRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Inquiry Source");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
