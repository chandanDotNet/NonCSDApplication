using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class RemoveCustomerImageCommandHandler : IRequestHandler<RemoveCustomerImageCommand, ServiceResponse<string>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<RemoveCustomerImageCommandHandler> _logger;

        public RemoveCustomerImageCommandHandler(ICustomerRepository customerRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<RemoveCustomerImageCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<string>> Handle(RemoveCustomerImageCommand request, CancellationToken cancellationToken)
        {
            var customerEntity = await _customerRepository.FindAsync(request.Id);
            if (customerEntity == null)
            {
                _logger.LogError("Customer not found.");
                return ServiceResponse<string>.Return404();
            }
            var customerImageUrl = customerEntity.Url;
            customerEntity.Url = string.Empty;
            _customerRepository.Update(customerEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while Removing Customer's image.");
                return ServiceResponse<string>.Return500();
            }
            return ServiceResponse<string>.ReturnResultWith200(customerImageUrl);
        }
    }
}
