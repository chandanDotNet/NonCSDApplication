using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DeleteSalesOrderCommandHandler
          : IRequestHandler<DeleteSalesOrderCommand, ServiceResponse<bool>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ILogger<DeleteSalesOrderCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IInventoryRepository _inventoryRepository;

        public DeleteSalesOrderCommandHandler(ISalesOrderRepository salesOrderRepository,
            ILogger<DeleteSalesOrderCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IInventoryRepository inventoryRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _logger = logger;
            _uow = uow;
            _inventoryRepository = inventoryRepository;
        }
        public async Task<ServiceResponse<bool>> Handle(DeleteSalesOrderCommand request, CancellationToken cancellationToken)
        {
            var salesOrder = await _salesOrderRepository.AllIncluding(c => c.SalesOrderItems).FirstOrDefaultAsync(c => c.Id == request.Id);

            if (salesOrder == null)
            {
                _logger.LogError("Sales order does not exists.");
                return ServiceResponse<bool>.Return404();
            }

            _salesOrderRepository.Delete(salesOrder);

            var inventories = salesOrder.SalesOrderItems
                .Select(cs => new InventoryDto
                {
                    ProductId = cs.ProductId,
                    PricePerUnit = cs.UnitPrice,
                    SalesOrderId = salesOrder.Id,
                    Stock = cs.Status == PurchaseSaleItemStatusEnum.Not_Return ? cs.Quantity : (-1) * cs.Quantity,
                    UnitId = cs.UnitId,
                    WarehouseId = cs.WarehouseId,
                    TaxValue = cs.TaxValue,
                    Discount = cs.Discount
                }).ToList();

            inventories.ForEach(invetory =>
            {
                invetory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(invetory);
            });

            var inventoriesToDelete = inventories
                .GroupBy(c => c.ProductId)
                .Select(cs => new InventoryDto
                {
                    InventorySource = InventorySourceEnum.DeleteSalesOrder,
                    ProductId = cs.Key,
                    PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
                    SalesOrderId = salesOrder.Id,
                    Stock = cs.Sum(d => d.Stock),
                }).ToList();

            foreach (var inventory in inventoriesToDelete)
            {
                await _inventoryRepository.AddInventory(inventory);
            }

            var warehouseInventoriesToAdd = inventories
                    .Where(c => c.WarehouseId.HasValue)
                    .GroupBy(c => new { c.WarehouseId, c.ProductId })
                    .Select(cs => new InventoryDto
                    {
                        InventorySource = InventorySourceEnum.DeleteSalesOrder,
                        ProductId = cs.Key.ProductId,
                        Stock = cs.Sum(d => d.Stock),
                        WarehouseId = cs.Key.WarehouseId
                    }).ToList();

            foreach (var inventory in warehouseInventoriesToAdd)
            {
                await _inventoryRepository.AddWarehouseInventory(inventory);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting Sales order.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }

}
