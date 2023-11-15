using MediatR;
using System;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateEmailTemplateCommand : IRequest<ServiceResponse<EmailTemplateDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
