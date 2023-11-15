using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class DeleteInquiryCommand : IRequest<ServiceResponse<InquiryDto>>
    {
        public Guid Id { get; set; }
    }
}
