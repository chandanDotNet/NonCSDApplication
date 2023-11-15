using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;


namespace POS.MediatR.UnitConversation.Commands
{
    public class AddUnitConversationCommand : IRequest<ServiceResponse<UnitConversationDto>>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Operator? Operator { get; set; }
        public Guid? ParentId { get; set; }
        public decimal? Value { get; set; }
    }
}
