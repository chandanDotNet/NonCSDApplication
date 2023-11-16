using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CustomerAddress.Commands;
using POS.MediatR.CustomerAddress.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.CustomerAddress.Handlers
{
    public class DeleteCustomerAddressCommandHandler : IRequestHandler<DeleteCustomerAddressCommand, ServiceResponse<bool>>
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteCustomerAddressCommandHandler> _logger;

        public DeleteCustomerAddressCommandHandler(
           ICustomerAddressRepository customerAddressRepository,
           IProductRepository productRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCustomerAddressCommandHandler> logger
            )
        {
            _customerAddressRepository = customerAddressRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _customerAddressRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Customer Address Does not exists");
                return ServiceResponse<bool>.Return404("Customer Address Does not exists");
            }
            _customerAddressRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Customer Address.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
