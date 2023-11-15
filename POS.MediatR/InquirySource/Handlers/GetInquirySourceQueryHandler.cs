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
    public class GetInquirySourceQueryHandler 
        : IRequestHandler<GetInquirySourceQuery, ServiceResponse<InquirySourceDto>>
    {
        private readonly IInquirySourceRepository _inquirySourceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetInquirySourceQueryHandler> _logger;

        public GetInquirySourceQueryHandler(
            IInquirySourceRepository inquirySourceRepository,
            IMapper mapper,
            ILogger<GetInquirySourceQueryHandler> logger)
        {
            _inquirySourceRepository = inquirySourceRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<InquirySourceDto>> Handle(GetInquirySourceQuery request, CancellationToken cancellationToken)
        {
            var entity = await _inquirySourceRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<InquirySourceDto>.ReturnResultWith200(_mapper.Map<InquirySourceDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<InquirySourceDto>.Return404();
            }
        }
    }
}
