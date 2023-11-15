using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Entities;
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
    public class AddInquiryStatusCommandHandler : IRequestHandler<AddInquiryStatusCommand, ServiceResponse<InquiryStatusDto>>
    {
        private readonly IInquiryStatusRepository _inquiryStatusRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddInquiryStatusCommandHandler> _logger;
        public AddInquiryStatusCommandHandler(
           IInquiryStatusRepository inquiryStatusRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddInquiryStatusCommandHandler> logger
            )
        {
            _inquiryStatusRepository = inquiryStatusRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<InquiryStatusDto>> Handle(AddInquiryStatusCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _inquiryStatusRepository.FindBy(c => c.Name.Trim().ToLower() == request.Name.Trim().ToLower()).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Inquiry Status already exist.");
                return ServiceResponse<InquiryStatusDto>.Return409("Inquiry Status already exist.");
            }
            var entity = _mapper.Map<InquiryStatus>(request);
            entity.Id = Guid.NewGuid();
            _inquiryStatusRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<InquiryStatusDto>.Return500();
            }
            var entityDto = _mapper.Map<InquiryStatusDto>(entity);
            return ServiceResponse<InquiryStatusDto>.ReturnResultWith200(entityDto);
        }
    }
}
