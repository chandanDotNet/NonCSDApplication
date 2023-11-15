using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateInquiryActivityCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOpen { get; set; }
        public Guid? AssignTo { get; set; }
        public string Priority { get; set; }
        public Guid InquiryId { get; set; }
    }
}
