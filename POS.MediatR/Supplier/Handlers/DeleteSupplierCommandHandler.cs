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
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, ServiceResponse<bool>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<AddSupplierCommandHandler> _logger;
        public DeleteSupplierCommandHandler(
            ISupplierRepository supplierRepository,
            IPurchaseOrderRepository purchaseOrderRepository,
            ILogger<AddSupplierCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow)
        {
            _supplierRepository = supplierRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _supplierRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Supplier is not exist");
                return ServiceResponse<bool>.Return422("Supplier does not exist");
            }
            var exitingSupplier = _purchaseOrderRepository.All.Any(c => c.SupplierId == entityExist.Id);
            if (exitingSupplier)
            {
                _logger.LogError("Supplier can not be Deleted because it is use in Purchase Order");
                return ServiceResponse<bool>.Return409("Supplier can not be Deleted because it is use in Purchase Order");
            }

            _supplierRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error to Delete Supplier");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
