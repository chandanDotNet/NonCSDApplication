using AutoMapper;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllInquiryStatusQueryHandler : IRequestHandler<GetAllInquiryStatusQuery, List<InquiryStatusDto>>
    {
        private readonly IInquiryStatusRepository _inquiryStatusRepository;
        private readonly IMapper _mapper;
        public GetAllInquiryStatusQueryHandler(
            IInquiryStatusRepository inquiryStatusRepository,
            IMapper mapper)
        {
            _inquiryStatusRepository = inquiryStatusRepository;
            _mapper = mapper;
        }

        public async Task<List<InquiryStatusDto>> Handle(GetAllInquiryStatusQuery request, CancellationToken cancellationToken)
        {
            var inquiries = await _inquiryStatusRepository.All.ToListAsync();
            return _mapper.Map<List<InquiryStatusDto>>(inquiries);
        }
    }
}
