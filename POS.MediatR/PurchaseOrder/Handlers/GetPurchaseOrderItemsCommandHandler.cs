using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrder.Handlers
{
    public class GetPurchaseOrderItemsCommandHandler : IRequestHandler<GetPurchaseOrderItemsCommand, List<PurchaseOrderItemDto>>
    {
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;

        public GetPurchaseOrderItemsCommandHandler(IPurchaseOrderItemRepository purchaseOrderItemRepository)
        {
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
        }

        public async Task<List<PurchaseOrderItemDto>> Handle(GetPurchaseOrderItemsCommand request, CancellationToken cancellationToken)
        {
            var itemsQuery = _purchaseOrderItemRepository.AllIncluding(c => c.Product.Unit, cs => cs.PurchaseOrderItemTaxes)
                .Where(c => c.PurchaseOrderId == request.Id);

            if (request.IsReturn)
            {
                itemsQuery = itemsQuery.Where(c => c.Status == Data.Entities.PurchaseSaleItemStatusEnum.Return);
            }
            
            var items = await itemsQuery
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new PurchaseOrderItemDto
                {
                    ProductName = c.Product.Name,
                    UnitName = c.UnitConversation.Name,
                    UnitPrice = c.UnitPrice,
                    Quantity = c.Quantity,
                    DiscountPercentage = c.DiscountPercentage,
                    Discount = c.Discount,
                    TaxValue = c.TaxValue,
                    ProductId = c.ProductId,
                    PurchaseOrderId = c.PurchaseOrderId,
                    Status = c.Status,
                    Id = c.Id,
                    UnitId = c.UnitId,
                    WarehouseId = c.WarehouseId,
                    WarehouseName= c.Warehouse.Name,
                    PurchaseOrderItemTaxes = c.PurchaseOrderItemTaxes.Select(c => new PurchaseOrderItemTaxDto
                    {
                        TaxName = c.Tax.Name,
                        TaxPercentage = c.Tax.Percentage,
                    }).ToList()
                })
                 .ToListAsync();
            return items;
        }
    }
}
