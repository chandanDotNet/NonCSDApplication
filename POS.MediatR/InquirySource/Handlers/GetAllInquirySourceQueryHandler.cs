using AutoMapper;
using POS.Data.Dto;
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
    public class GetAllInquirySourceQueryHandler
        : IRequestHandler<GetAllInquirySourceQuery, List<InquirySourceDto>>
    {
        private readonly IInquirySourceRepository _inquirySourceRepository;
        private readonly IMapper _mapper;

        public GetAllInquirySourceQueryHandler(
            IInquirySourceRepository inquirySourceRepository,
            IMapper mapper)
        {
            _inquirySourceRepository = inquirySourceRepository;
            _mapper = mapper;
        }
        public async Task<List<InquirySourceDto>> Handle(GetAllInquirySourceQuery request, CancellationToken cancellationToken)
        {
            var entities = await _inquirySourceRepository.All.ToListAsync();
            var dtoEntities = _mapper.Map<List<InquirySourceDto>>(entities);
            return dtoEntities;
        }
    }
}
