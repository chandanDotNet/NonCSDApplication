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
    public class SalesOrderItemList : List<SalesOrderItemDto>
    {
        public SalesOrderItemList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public SalesOrderItemList(List<SalesOrderItemDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<SalesOrderItemList> Create(IQueryable<SalesOrderItem> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new SalesOrderItemList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<SalesOrderItem> source)
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

        public async Task<List<SalesOrderItemDto>> GetDtos(IQueryable<SalesOrderItem> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                             .AsNoTracking()
                             .Select(c => new SalesOrderItemDto
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
                                 SalesOrderId = c.SalesOrderId,
                                 SalesOrderNumber = c.SalesOrder.OrderNumber,
                                 CustomerName = c.SalesOrder.Customer.CustomerName,
                                 SOCreatedDate = c.SalesOrder.SOCreatedDate,
                                 Status = c.Status,
                                 Id = c.Id,
                                 Total = (c.UnitPrice * c.Quantity) - c.Discount + c.TaxValue,
                                 SalesOrderItemTaxes = c.SalesOrderItemTaxes.Select(c => new SalesOrderItemTaxDto
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
                             .Select(c => new SalesOrderItemDto
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
                                 SalesOrderId = c.SalesOrderId,
                                 SalesOrderNumber = c.SalesOrder.OrderNumber,
                                 CustomerName = c.SalesOrder.Customer.CustomerName,
                                 SOCreatedDate = c.SalesOrder.SOCreatedDate,
                                 Status = c.Status,
                                 Id = c.Id,
                                 Total = (c.UnitPrice * c.Quantity) - c.Discount + c.TaxValue,
                                 SalesOrderItemTaxes = c.SalesOrderItemTaxes.Select(c => new SalesOrderItemTaxDto
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
