using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.Product.Command;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using POS.Data;

namespace POS.MediatR.Product.Handler
{
    public class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand, ServiceResponse<bool>>
    {

        private readonly IProductRepository _productRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(IProductRepository productRepository,
            IUnitOfWork<POSDbContext> uow,
            IPurchaseOrderRepository purchaseOrderRepository,
            ISalesOrderRepository salesOrderRepository,
            ILogger<DeleteProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _salesOrderRepository = salesOrderRepository;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.FindAsync(request.Id);
            if (existingProduct == null)
            {
                _logger.LogError("Proudct does not exists.");
                return ServiceResponse<bool>.Return404("Proudct does not exists.");
            }

            var exitingPurchaseOrder = _purchaseOrderRepository
               .AllIncluding(c => c.PurchaseOrderItems)
               .Where(c => c.PurchaseOrderItems.Any(c => c.ProductId == existingProduct.Id)).Any();

            if (exitingPurchaseOrder)
            {
                _logger.LogError("Proudct can not be Deleted because it is use in Purchase Order");
                return ServiceResponse<bool>.Return409("Proudct can not be Deleted because it is use in Purchase Order");
            }

            var exitingSalesOrder = _salesOrderRepository
               .AllIncluding(c => c.SalesOrderItems)
               .Where(c => c.SalesOrderItems.Any(c => c.ProductId == existingProduct.Id)).Any();

            if (exitingSalesOrder)
            {
                _logger.LogError("Proudct can not be Deleted because it is use in Sales Order");
                return ServiceResponse<bool>.Return409("Proudct can not be Deleted because it is use in Sales Order");
            }
            
            _productRepository.Delete(existingProduct);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While deleting Proudct.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
