using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.Tax.Commands;

namespace POS.MediatR.Tax.Handlers
{
    public class DeleteTaxCommandHandler
        : IRequestHandler<DeleteTaxCommand, ServiceResponse<bool>>
    {
        private readonly ITaxRepository _taxRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteTaxCommandHandler> _logger;

        public DeleteTaxCommandHandler(
           ITaxRepository taxRepository,
           IProductRepository productRepository,
           ISalesOrderItemRepository salesOrderItemRepository,
           IPurchaseOrderItemRepository purchaseOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
             ILogger<DeleteTaxCommandHandler> logger
            )
        {
            _taxRepository = taxRepository;
            _productRepository = productRepository;
            _uow = uow;
            _logger = logger;
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteTaxCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _taxRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Tax Does not exists");
                return ServiceResponse<bool>.Return404("Tax  Does not exists");
            }

            var exitingProduct = _productRepository.AllIncluding(c => c.ProductTaxes).Where(c => c.ProductTaxes.Any(c => c.TaxId == entityExist.Id)).Any();
            if (exitingProduct)
            {
                _logger.LogError("Tax can not be Deleted because it is use in product");
                return ServiceResponse<bool>.Return409("Tax can not be Deleted because it is use in product.");
            }

            var exitingPurchaseOrder = _purchaseOrderItemRepository
                .AllIncluding(c => c.PurchaseOrderItemTaxes)
                .Where(c => !c.PurchaseOrder.IsDeleted && c.PurchaseOrderItemTaxes.Any(c => c.TaxId == entityExist.Id)).Any();
            if (exitingPurchaseOrder)
            {
                _logger.LogError("Tax can not be Deleted because it is use in Purchase Order");
                return ServiceResponse<bool>.Return409("Tax can not be Deleted because it is use in Purchase Order");
            }

            var exitingSalesOrder = _salesOrderItemRepository
               .AllIncluding(c => c.SalesOrderItemTaxes)
               .Where(c => !c.SalesOrder.IsDeleted && c.SalesOrderItemTaxes.Any(c => c.TaxId == entityExist.Id)).Any();
            if (exitingPurchaseOrder)
            {
                _logger.LogError("Tax can not be Deleted because it is use in Sales Order");
                return ServiceResponse<bool>.Return409("Tax can not be Deleted because it is use in Sales Order");
            }

            _taxRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Tax.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
