using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.UnitConversation.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.UnitConversation.Handlers
{
    public class GetAllUnitConversationCommandHandlers : IRequestHandler<GetAllUnitConversationCommand, List<UnitConversationDto>>
    {
        private readonly IUnitConversationRepository _unitConversationRepository;
        private readonly IMapper _mapper;

        public GetAllUnitConversationCommandHandlers(
            IUnitConversationRepository unitConversationRepository,
            IMapper mapper)
        {
            _unitConversationRepository = unitConversationRepository;
            _mapper = mapper;
        }
        public async Task<List<UnitConversationDto>> Handle(GetAllUnitConversationCommand request, CancellationToken cancellationToken)
        {
            var units = await _unitConversationRepository.AllIncluding(c=>c.Parent)
                .OrderBy(c => c.Name)
                .Select(c=> new UnitConversationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    BaseUnitName = c.Parent !=null? c.Parent.Name + "(" + c.Parent.Code  + ")" : "",
                    Value = c.Value,
                    Operator =c.Operator,
                    Code = c.Code,
                })
                .ToListAsync();
            return units;
        }
    }
}
