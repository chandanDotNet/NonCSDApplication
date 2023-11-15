using MediatR;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class AddEmailTemplateCommand : IRequest<ServiceResponse<EmailTemplateDto>>
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
