using MediatR;
using System;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class GetEmailTemplateQuery : IRequest<ServiceResponse<EmailTemplateDto>>
    {
        public Guid Id { get; set; }
    }
}
