using MediatR;
using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.UnitConversation.Commands
{
    public class UpdateUnitConversationCommand : IRequest<ServiceResponse<UnitConversationDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Operator? Operator { get; set; }
        public decimal? Value { get; set; }
        public Guid? ParentId { get; set; }
    }
}
