using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
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
    public class UpdateInquiryStatusCommandHandler
        : IRequestHandler<UpdateInquiryStatusCommand, ServiceResponse<InquiryStatusDto>>
    {
        private readonly IInquiryStatusRepository _inquiryStatusRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInquiryStatusCommandHandler> _logger;
        public UpdateInquiryStatusCommandHandler(
           IInquiryStatusRepository inquiryStatusRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateInquiryStatusCommandHandler> logger
            )
        {
            _inquiryStatusRepository = inquiryStatusRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<InquiryStatusDto>> Handle(UpdateInquiryStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _inquiryStatusRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Inquiry Status Already Exist.");
                return ServiceResponse<InquiryStatusDto>.Return409("Inquiry Status Already Exist.");
            }

            entityExist = await _inquiryStatusRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.Name = request.Name;
            _inquiryStatusRepository.Update(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<InquiryStatusDto>.Return500();
            }

            var entityDto = _mapper.Map<InquiryStatusDto>(entityExist);
            return ServiceResponse<InquiryStatusDto>.ReturnResultWith200(entityDto);
        }
    }
}
