using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetInquiryActivitiesQueryHandler : IRequestHandler<GetInquiryActivitiesQuery, List<InquiryActivityDto>>
    {
        private readonly IInquiryActivityRepository _inquiryActivityRepository;
        private readonly IMapper _mapper;

        public GetInquiryActivitiesQueryHandler(
            IInquiryActivityRepository inquiryActivityRepository,
            IMapper mapper,
            ILogger<GetInquiryActivitiesQueryHandler> logger)
        {
            _inquiryActivityRepository = inquiryActivityRepository;
            _mapper = mapper;
        }
        public async Task<List<InquiryActivityDto>> Handle(GetInquiryActivitiesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _inquiryActivityRepository.All
                .Include(c => c.AssignUser)
                .Where(c => c.InquiryId == request.InquiryId)
                .ToListAsync();

            return _mapper.Map<List<InquiryActivityDto>>(entities);
        }
    }
}
