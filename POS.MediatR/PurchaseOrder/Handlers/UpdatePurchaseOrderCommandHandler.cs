using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.PurchaseOrder.Commands;
using POS.Repository;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrder.Handlers
{
    public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommand, ServiceResponse<PurchaseOrderDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePurchaseOrderCommandHandler> _logger;
        private readonly IInventoryRepository _inventoryRepository;

        public UpdatePurchaseOrderCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderItemRepository purchaseOrderItemRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<UpdatePurchaseOrderCommandHandler> logger,
            IInventoryRepository inventoryRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<ServiceResponse<PurchaseOrderDto>> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var existingPONumber = _purchaseOrderRepository.All.Any(c => c.OrderNumber == request.OrderNumber && c.Id != request.Id);
            if (existingPONumber)
            {
                return ServiceResponse<PurchaseOrderDto>.Return409("Purchase Order Number is already Exists.");
            }

            var purchaseOrderExit = await _purchaseOrderRepository.FindAsync(request.Id);
            if (purchaseOrderExit.Status == Data.PurchaseOrderStatus.Return)
            {
                return ServiceResponse<PurchaseOrderDto>.Return409("Purchase Order can't edit becuase it's already approved.");
            }

            var purchaseOrderItemsExist = await _purchaseOrderItemRepository.FindBy(c => c.PurchaseOrderId == request.Id).ToListAsync();

            // remove existing warehouse inventory
            if (!request.IsPurchaseOrderRequest)
            {
                var inventories = purchaseOrderItemsExist
                    .Where(c => c.WarehouseId.HasValue)
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

                var warehouseInventories = inventories
                     .GroupBy(c => new { c.WarehouseId, c.ProductId })
                     .Select(cs => new InventoryDto
                     {
                         InventorySource = InventorySourceEnum.PurchaseOrder,
                         ProductId = cs.Key.ProductId,
                         Stock = cs.Sum(d => d.Stock),
                         WarehouseId = cs.Key.WarehouseId
                     }).ToList();

                if (warehouseInventories.Any())
                {
                    await _inventoryRepository.RemoveExistingWareHouseInventory(warehouseInventories);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        _logger.LogError("Error while Updating Purchase Order.");
                        return ServiceResponse<PurchaseOrderDto>.Return500();
                    }
                }
            }

            _purchaseOrderItemRepository.RemoveRange(purchaseOrderItemsExist);

            var purchaseOrderUpdate = _mapper.Map<POS.Data.PurchaseOrder>(request);
            purchaseOrderExit.OrderNumber = purchaseOrderUpdate.OrderNumber;
            purchaseOrderExit.SupplierId = purchaseOrderUpdate.SupplierId;
            purchaseOrderExit.Note = purchaseOrderUpdate.Note;
            purchaseOrderExit.TermAndCondition = purchaseOrderUpdate.TermAndCondition;
            purchaseOrderExit.IsPurchaseOrderRequest = purchaseOrderUpdate.IsPurchaseOrderRequest;
            purchaseOrderExit.POCreatedDate = purchaseOrderUpdate.POCreatedDate;
            purchaseOrderExit.Status = purchaseOrderUpdate.Status;
            purchaseOrderExit.DeliveryDate = purchaseOrderUpdate.DeliveryDate;
            purchaseOrderExit.DeliveryStatus = purchaseOrderUpdate.DeliveryStatus;
            purchaseOrderExit.SupplierId = purchaseOrderUpdate.SupplierId;
            purchaseOrderExit.TotalAmount = purchaseOrderUpdate.TotalAmount;
            purchaseOrderExit.TotalTax = purchaseOrderUpdate.TotalTax;
            purchaseOrderExit.TotalDiscount = purchaseOrderUpdate.TotalDiscount;
            purchaseOrderExit.PurchaseOrderItems = purchaseOrderUpdate.PurchaseOrderItems;
            purchaseOrderExit.PurchaseOrderItems.ForEach(c =>
            {
                c.PurchaseOrderId = purchaseOrderUpdate.Id;
            });
            purchaseOrderExit.PurchaseOrderItems.ForEach(item =>
            {
                item.Product = null;
                item.Warehouse = null;
                item.PurchaseOrderItemTaxes.ForEach(tax => { tax.Tax = null; });
            });
            _purchaseOrderRepository.Update(purchaseOrderExit);

            if (!request.IsPurchaseOrderRequest)
            {
                var inventories = request.PurchaseOrderItems
                    .Select(cs => new InventoryDto
                    {
                        ProductId = cs.ProductId,
                        PricePerUnit = cs.UnitPrice,
                        PurchaseOrderId = purchaseOrderUpdate.Id,
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
                        InventorySource = Data.InventorySourceEnum.PurchaseOrder,
                        ProductId = cs.Key,
                        PricePerUnit = cs.Sum(d => d.PricePerUnit * d.Stock + d.TaxValue - d.Discount) / cs.Sum(d => d.Stock),
                        PurchaseOrderId = purchaseOrderUpdate.Id,
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
            var dto = _mapper.Map<PurchaseOrderDto>(purchaseOrderExit);
            return ServiceResponse<PurchaseOrderDto>.ReturnResultWith201(dto);
        }
    }
}
