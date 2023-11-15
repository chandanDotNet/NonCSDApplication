using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Domain;
using POS.Helper;
using POS.MediatR.PurchaseOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrder.Handlers
{
    public class UpdatePurchaseOrderReturnCommandHandler : IRequestHandler<UpdatePurchaseOrderReturnCommand, ServiceResponse<bool>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePurchaseOrderReturnCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public UpdatePurchaseOrderReturnCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderItemRepository purchaseOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdatePurchaseOrderReturnCommandHandler> logger,
            IInventoryRepository inventoryRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdatePurchaseOrderReturnCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrderExit = _purchaseOrderRepository.AllIncluding(c => c.PurchaseOrderItems).FirstOrDefault(c => c.Id == request.Id);

            var purchaseOrderUpdate = _mapper.Map<Data.PurchaseOrder>(request);

            if (purchaseOrderExit.Status == Data.PurchaseOrderStatus.Return)
            {
                return ServiceResponse<bool>.Return409("Purchase Order can't edit becuase it's already approved.");
            }

            purchaseOrderExit.Status = Data.PurchaseOrderStatus.Return;
            purchaseOrderExit.TotalAmount = purchaseOrderExit.TotalAmount - purchaseOrderUpdate.TotalAmount;
            purchaseOrderExit.TotalTax = purchaseOrderExit.TotalTax - purchaseOrderUpdate.TotalTax;
            purchaseOrderExit.TotalDiscount = purchaseOrderExit.TotalDiscount - purchaseOrderUpdate.TotalDiscount;
            purchaseOrderExit.PurchaseOrderItems = purchaseOrderUpdate.PurchaseOrderItems;
            purchaseOrderExit.PurchaseReturnNote = purchaseOrderUpdate.Note;

            purchaseOrderUpdate.PurchaseOrderItems.ForEach(c =>
            {
                c.PurchaseOrderId = purchaseOrderUpdate.Id;
            });
            purchaseOrderExit.PurchaseOrderItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.PurchaseOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
                item.CreatedDate = DateTime.UtcNow;
                item.Status = PurchaseSaleItemStatusEnum.Return;
            });

            if (purchaseOrderExit.TotalAmount <= purchaseOrderExit.TotalPaidAmount)
            {
                purchaseOrderExit.PaymentStatus = PaymentStatus.Paid;
            }
            else if (purchaseOrderExit.TotalPaidAmount > 0)
            {
                purchaseOrderExit.PaymentStatus = PaymentStatus.Partial;
            }
            else
            {
                purchaseOrderExit.PaymentStatus = PaymentStatus.Pending;
            }
            _purchaseOrderRepository.Update(purchaseOrderExit);

            var inventories = request.PurchaseOrderItems
                .Select(cs => new InventoryDto
                {
                    ProductId = cs.ProductId,
                    PricePerUnit = cs.UnitPrice,
                    PurchaseOrderId = purchaseOrderExit.Id,
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
                    InventorySource = InventorySourceEnum.PurchaseOrderReturn,
                    ProductId = cs.Key,
                    PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
                    PurchaseOrderId = purchaseOrderExit.Id,
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
                    InventorySource = InventorySourceEnum.PurchaseOrderReturn,
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
                _logger.LogError("Error while creating Purchase Order.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith201(true);
        }
    }
}

