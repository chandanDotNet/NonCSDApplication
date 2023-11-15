using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
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
    public class DeleteUnitConversationCommandHandlers : IRequestHandler<DeleteUnitConversationCommand, ServiceResponse<bool>>
    {
        private readonly IUnitConversationRepository _unitConversationRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteUnitConversationCommandHandlers> _logger;
        public DeleteUnitConversationCommandHandlers(
          IUnitConversationRepository unitConversationRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteUnitConversationCommandHandlers> logger
            )
        {
            _unitConversationRepository= unitConversationRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteUnitConversationCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _unitConversationRepository
                .FindAsync(request.Id);

            if (existingEntity == null)
            {
                _logger.LogError("Unit Conversation not Exists");
                return ServiceResponse<bool>.Return409("Unit Conversation not Exists.");
            }

            _unitConversationRepository.Delete(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Unit Conversation.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}