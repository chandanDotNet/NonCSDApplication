using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class DeleteInquiryAttachmentCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
