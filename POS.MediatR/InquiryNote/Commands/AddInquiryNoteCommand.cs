using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class AddInquiryNoteCommand: IRequest<ServiceResponse<InquiryNoteDto>>
    {
        public Guid InquiryId { get; set; }
        public string Note { get; set; }
    }
}
