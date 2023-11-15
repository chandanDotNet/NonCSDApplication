using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetInquiryAttachmentsByInquiryIdQuery : IRequest<List<InquiryAttachmentDto>>
    {
        public Guid InquiryId { get; set; }
    }
}
