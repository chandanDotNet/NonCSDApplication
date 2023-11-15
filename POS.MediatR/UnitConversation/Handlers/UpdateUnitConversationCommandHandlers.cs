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
    public class UpdateUnitConversationCommandHandlers : IRequestHandler<UpdateUnitConversationCommand, ServiceResponse<UnitConversationDto>>
    {
        private readonly IUnitConversationRepository _unitConversationRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUnitConversationCommandHandlers> _logger;
        public UpdateUnitConversationCommandHandlers(
            IUnitConversationRepository unitConversationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateUnitConversationCommandHandlers> logger
            )
        {
            _unitConversationRepository = unitConversationRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<UnitConversationDto>> Handle(UpdateUnitConversationCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _unitConversationRepository
                .All
                .FirstOrDefaultAsync(c => c.Name == request.Name
                && c.ParentId == request.ParentId
                && c.Id != request.Id);
            if (existingEntity != null)
            {
                _logger.LogError("Unit Conversation Already Exist");
                return ServiceResponse<UnitConversationDto>.Return409("Unit Conversation Already Exist.");
            }

            existingEntity = await _unitConversationRepository.FindAsync(request.Id);
            var entity = _mapper.Map(request, existingEntity);
            _unitConversationRepository.Update(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Product Category.");
                return ServiceResponse<UnitConversationDto>.Return500();
            }
            return ServiceResponse<UnitConversationDto>.ReturnResultWith200(_mapper.Map<UnitConversationDto>(entity));
        }
    }
}
