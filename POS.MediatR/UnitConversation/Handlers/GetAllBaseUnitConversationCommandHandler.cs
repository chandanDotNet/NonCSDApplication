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
    public class GetAllBaseUnitConversationCommandHandler : IRequestHandler<GetAllBaseUnitConversationCommand, List<UnitConversationDto>>
    {
        private readonly IUnitConversationRepository _unitConversationRepository;
        private readonly IMapper _mapper;

        public GetAllBaseUnitConversationCommandHandler(
            IUnitConversationRepository unitConversationRepository,
            IMapper mapper)
        {
            _unitConversationRepository = unitConversationRepository;
            _mapper = mapper;
        }
        public async Task<List<UnitConversationDto>> Handle(GetAllBaseUnitConversationCommand request, CancellationToken cancellationToken)
        {
            var units = await _unitConversationRepository.All.Where(c => c.ParentId == null)
                .OrderBy(c => c.Name)
                .ProjectTo<UnitConversationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return units;
        }
    }
}
