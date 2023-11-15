using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddPurchaseOrderCommandHandler : IRequestHandler<AddPurchaseOrderCommand, ServiceResponse<PurchaseOrderDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPurchaseOrderCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public AddPurchaseOrderCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddPurchaseOrderCommandHandler> logger,
            IInventoryRepository inventoryRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ServiceResponse<PurchaseOrderDto>> Handle(AddPurchaseOrderCommand request, CancellationToken cancellationToken)
        {

            var existingPONumber = _purchaseOrderRepository.All.Any(c => c.OrderNumber == request.OrderNumber);
            if (existingPONumber)
            {
                return ServiceResponse<PurchaseOrderDto>.Return409("Purchase Order Number is already Exists.");
            }

            var purchaseOrder = _mapper.Map<Data.PurchaseOrder>(request);
            purchaseOrder.PaymentStatus = PaymentStatus.Pending;
            purchaseOrder.PurchaseOrderItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.PurchaseOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
                item.CreatedDate = DateTime.UtcNow;
            });
            _purchaseOrderRepository.Add(purchaseOrder);

            if (!request.IsPurchaseOrderRequest)
            {
                var inventories = purchaseOrder.PurchaseOrderItems
                   .Select(cs => new InventoryDto
                   {
                       ProductId = cs.ProductId,
                       PricePerUnit = cs.UnitPrice,
                       PurchaseOrderId = purchaseOrder.Id,
                       Stock = cs.Quantity,
                       UnitId = cs.UnitId,
                       WarehouseId = cs.WarehouseId,
                       TaxValue = cs.TaxValue,
                       Discount = cs.Discount
                   }).ToList();

                inventories.ForEach(invetory =>
                {
                    invetory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(invetory);
                });

                var inventoriesToAdd = inventories
                    .GroupBy(c => c.ProductId)
                    .Select(cs => new InventoryDto
                    {
                        InventorySource = InventorySourceEnum.PurchaseOrder,
                        ProductId = cs.Key,
                        PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
                        PurchaseOrderId = purchaseOrder.Id,
                        Stock = cs.Sum(d => d.Stock),
                    }).ToList();

                foreach (var inventory in inventoriesToAdd)
                {
                    await _inventoryRepository.AddInventory(inventory);
                }

                var warehouseInventoriesToAdd = inventories
                    .Where(c => c.WarehouseId.HasValue)
                    .GroupBy(c => new { c.WarehouseId, c.ProductId })
                    .Select(cs => new InventoryDto
                    {
                        InventorySource = InventorySourceEnum.PurchaseOrder,
                        ProductId = cs.Key.ProductId,
                        Stock = cs.Sum(d => d.Stock),
                        WarehouseId = cs.Key.WarehouseId
                    }).ToList();

                foreach (var inventory in warehouseInventoriesToAdd)
                {
                    await _inventoryRepository.AddWarehouseInventory(inventory);
                }
            }

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Purchase Order.");
                return ServiceResponse<PurchaseOrderDto>.Return500();
            }
            var dto = _mapper.Map<PurchaseOrderDto>(purchaseOrder);
            return ServiceResponse<PurchaseOrderDto>.ReturnResultWith201(dto);
        }
    }
}
