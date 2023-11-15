using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;

namespace POS.MediatR.Handlers
{
    public class DeleteActionCommandHandler : IRequestHandler<DeleteActionCommand, ServiceResponse<ActionDto>>
    {
        private readonly IActionRepository _actionRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteActionCommandHandler> _logger;
        public DeleteActionCommandHandler(
           IActionRepository actionRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteActionCommandHandler> logger
            )
        {
            _actionRepository = actionRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<ActionDto>> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _actionRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Not found");
                return ServiceResponse<ActionDto>.Return404();
            }

            _actionRepository.Delete(request.Id);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<ActionDto>.Return500();
            }

            return ServiceResponse<ActionDto>.ReturnSuccess();
        }
    }
}
