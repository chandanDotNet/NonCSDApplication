using MediatR;
using System;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class DeleteEmailSMTPSettingCommand : IRequest<ServiceResponse<EmailSMTPSettingDto>>
    {
        public Guid Id { get; set; }
    }
}
