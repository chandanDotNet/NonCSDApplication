using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace POS.MediatR.Handlers
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, ServiceResponse<bool>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteCustomerCommandHandler> _logger;

        public DeleteCustomerCommandHandler(
            ICustomerRepository customerRepository,
            ISalesOrderRepository salesOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _salesOrderRepository = salesOrderRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _customerRepository.FindAsync(request.Id);
          
            if (entityExist.IsWalkIn)
            {
                _logger.LogError("Customer can not be Deleted because it is Walk In Customer");
                return ServiceResponse<bool>.Return409("Customer can not be Deleted because it is Walk In Customer");
            }

            if (entityExist == null)
            {
                _logger.LogError("Customer not found");
                return ServiceResponse<bool>.Return404();
            }

            var exitingSupplier = _salesOrderRepository.All.Any(c => c.CustomerId == entityExist.Id);
            if (exitingSupplier)
            {
                _logger.LogError("Customer can not be Deleted because it is use in Sales Order");
                return ServiceResponse<bool>.Return409("Customer can not be Deleted because it is use in Sales Order");
            }
            _customerRepository.Delete(entityExist);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting the Customer.", request.Id);
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
