using AutoMapper;
using POS.Data.Dto;
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

    public class GetInquiryStatusQueryHandler : IRequestHandler<GetInquiryStatusQuery, ServiceResponse<InquiryStatusDto>>
    {
        private readonly IInquiryStatusRepository _inquiryStatusRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetInquiryStatusQueryHandler> _logger;

        public GetInquiryStatusQueryHandler(
           IInquiryStatusRepository inquiryStatusRepository,
           IMapper mapper,
           ILogger<GetInquiryStatusQueryHandler> logger)
        {
            _inquiryStatusRepository = inquiryStatusRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<InquiryStatusDto>> Handle(GetInquiryStatusQuery request, CancellationToken cancellationToken)
        {
            var entity = await _inquiryStatusRepository.FindAsync(request.Id);
            if (entity == null)
            {
                _logger.LogError("Inquiry Status not found");
                return ServiceResponse<InquiryStatusDto>.Return404();
            }
            else
                return ServiceResponse<InquiryStatusDto>.ReturnResultWith200(_mapper.Map<InquiryStatusDto>(entity));
        }
    }
}
