using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class PurchaseOrderItemList : List<PurchaseOrderItemDto>
    {
        public PurchaseOrderItemList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PurchaseOrderItemList(List<PurchaseOrderItemDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<PurchaseOrderItemList> Create(IQueryable<PurchaseOrderItem> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new PurchaseOrderItemList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<PurchaseOrderItem> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<PurchaseOrderItemDto>> GetDtos(IQueryable<PurchaseOrderItem> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
             .AsNoTracking()
             .Select(c => new PurchaseOrderItemDto
             {
                 ProductName = c.Product.Name,
                 UnitName = c.Product.Unit.Name,
                 UnitPrice = c.UnitPrice,
                 Quantity = c.Quantity,
                 DiscountPercentage = c.DiscountPercentage,
                 Discount = c.Discount,
                 TaxValue = c.TaxValue,
                 ProductId = c.ProductId,
                 WarehouseId = c.WarehouseId,
                 WarehouseName = c.Warehouse.Name,
                 PurchaseOrderId = c.PurchaseOrderId,
                 PurchaseOrderNumber= c.PurchaseOrder.OrderNumber,
                 SupplierName = c.PurchaseOrder.Supplier.SupplierName,
                 POCreatedDate = c.PurchaseOrder.POCreatedDate,
                 Status = c.Status,
                 Id = c.Id,
                 Total= (c.UnitPrice * c.Quantity) - c.Discount + c.TaxValue,
                 PurchaseOrderItemTaxes = c.PurchaseOrderItemTaxes.Select(c => new PurchaseOrderItemTaxDto
                 {
                     TaxName = c.Tax.Name,
                     TaxPercentage = c.Tax.Percentage,
                 }).ToList()
             })
             .ToListAsync();
                return entities;
            }
            else
            {

                var entities = await source
             .Skip(skip)
             .Take(pageSize)
             .AsNoTracking()
             .Select(c => new PurchaseOrderItemDto
             {
                 ProductName = c.Product.Name,
                 UnitName = c.Product.Unit.Name,
                 UnitPrice = c.UnitPrice,
                 Quantity = c.Quantity,
                 DiscountPercentage = c.DiscountPercentage,
                 Discount = c.Discount,
                 TaxValue = c.TaxValue,
                 ProductId = c.ProductId,
                 PurchaseOrderId = c.PurchaseOrderId,
                 POCreatedDate = c.PurchaseOrder.POCreatedDate,
                 PurchaseOrderNumber = c.PurchaseOrder.OrderNumber,
                 SupplierName= c.PurchaseOrder.Supplier.SupplierName,
                 Status = c.Status,
                 WarehouseId = c.WarehouseId,
                 WarehouseName = c.Warehouse.Name,
                 Id = c.Id,
                 Total = (c.UnitPrice * c.Quantity) - c.Discount + c.TaxValue,
                 PurchaseOrderItemTaxes = c.PurchaseOrderItemTaxes.Select(c => new PurchaseOrderItemTaxDto
                 {
                     TaxName = c.Tax.Name,
                     TaxPercentage = c.Tax.Percentage,
                 }).ToList()
             })
             .ToListAsync();
                return entities;
            }

        }
    }
}

