using POS.Data.Dto;
using POS.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class AddInquiryAttachmentCommand : IRequest<ServiceResponse<InquiryAttachmentDto>>
    {
        public Guid InquiryId { get; set; }
        public string Name { get; set; }
        public string Documents { get; set; }
        public string Extension { get; set; }
    }
}
