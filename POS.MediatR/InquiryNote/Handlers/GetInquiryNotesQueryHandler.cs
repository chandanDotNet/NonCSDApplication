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
    public class GetInquiryNotesQueryHandler : IRequestHandler<GetInquiryNotesQuery, List<InquiryNoteDto>>
    {
        private readonly IInquiryNoteRepository _inquiryNoteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetInquiryNotesQueryHandler> _logger;
        public GetInquiryNotesQueryHandler(
            IInquiryNoteRepository inquiryNoteRepository,
            IMapper mapper,
            ILogger<GetInquiryNotesQueryHandler> logger)
        {
            _inquiryNoteRepository = inquiryNoteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<InquiryNoteDto>> Handle(GetInquiryNotesQuery request, CancellationToken cancellationToken)
        {
            var inquiryNotes = await _inquiryNoteRepository.All
                .Include(c => c.CreatedByUser)
                .Where(c => c.InquiryId == request.InquiryId).ToListAsync();
            var inquiryNotesDto = _mapper.Map<List<InquiryNoteDto>>(inquiryNotes);
            return inquiryNotesDto;
        }
    }
}
