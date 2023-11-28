using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Counter.Commands;
using POS.MediatR.Counter.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Handlers
{
    public class DeleteCounterCommandHandler
      : IRequestHandler<DeleteCounterCommand, ServiceResponse<bool>>
    {
        private readonly ICounterRepository _counterRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteCounterCommandHandler> _logger;

        public DeleteCounterCommandHandler(
           ICounterRepository counterRepository,
           IUserRepository userRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCounterCommandHandler> logger
            )
        {
            _counterRepository = counterRepository;
            _userRepository = userRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteCounterCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _counterRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Counter Does not exists");
                return ServiceResponse<bool>.Return404("Counter  Does not exists");
            }

            var exitingProduct = _userRepository.AllIncluding(c => c.Counter).Any(c => c.Counter.Id == entityExist.Id);
            if (exitingProduct)
            {
                _logger.LogError("Counter can not be Deleted because it is use in user");
                return ServiceResponse<bool>.Return409("Counter can not be Deleted because it is use in user.");
            }

            _counterRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Brand.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
