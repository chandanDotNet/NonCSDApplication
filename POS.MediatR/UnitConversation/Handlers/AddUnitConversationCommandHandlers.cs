using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
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
    public class AddUnitConversationCommandHandlers : IRequestHandler<AddUnitConversationCommand, ServiceResponse<UnitConversationDto>>
    {
        private readonly IUnitConversationRepository _unitConversationRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddUnitConversationCommandHandlers> _logger;
        public AddUnitConversationCommandHandlers(
           IUnitConversationRepository unitConversationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddUnitConversationCommandHandlers> logger
            )
        {
            _unitConversationRepository = unitConversationRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<UnitConversationDto>> Handle(AddUnitConversationCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _unitConversationRepository
                .All
                .FirstOrDefaultAsync(c => c.Name == request.Name && c.ParentId == request.ParentId);

            if (existingEntity != null)
            {
                _logger.LogError("Unit Conversation Already Exist");
                return ServiceResponse<UnitConversationDto>.Return409("Unit Conversation Already Exist.");
            }
            var entity = _mapper.Map<Data.UnitConversation>(request);
            entity.Id = Guid.NewGuid();
            _unitConversationRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Unit Conversation.");
                return ServiceResponse<UnitConversationDto>.Return500();
            }
            return ServiceResponse<UnitConversationDto>.ReturnResultWith200(_mapper.Map<UnitConversationDto>(entity));
        }
    }
}
