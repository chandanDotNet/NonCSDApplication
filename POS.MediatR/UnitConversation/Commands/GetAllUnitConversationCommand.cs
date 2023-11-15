using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.UnitConversation.Commands
{
    public class GetAllUnitConversationCommand : IRequest<List<UnitConversationDto>>
    {
        public Guid? Id { get; set; }
    }
}
