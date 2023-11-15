using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetInquiryAttachmentsByInquiryIdQueryHandler : IRequestHandler<GetInquiryAttachmentsByInquiryIdQuery, List<InquiryAttachmentDto>>
    {

        private readonly IInquiryAttachmentRepository _inquiryAttachmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetInquiryAttachmentsByInquiryIdQueryHandler(
            IInquiryAttachmentRepository inquiryAttachmentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            PathHelper pathHelper
          )
        {
            _inquiryAttachmentRepository = inquiryAttachmentRepository;
            _uow = uow;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<InquiryAttachmentDto>> Handle(GetInquiryAttachmentsByInquiryIdQuery request, CancellationToken cancellationToken)
        {
            var inquiryAttachments = await _inquiryAttachmentRepository.All.Include(c => c.CreatedByUser)
                .Where(c => c.InquiryId == request.InquiryId)
                .ToListAsync();

            var inquiryAttachmentsDto = _mapper.Map<List<InquiryAttachmentDto>>(inquiryAttachments);

            return inquiryAttachmentsDto;
        }
    }
}
