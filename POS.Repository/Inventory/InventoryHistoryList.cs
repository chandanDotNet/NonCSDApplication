using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class InventoryHistoryList : List<InventoryHistoryDto>
    {
        public InventoryHistoryList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public InventoryHistoryList(List<InventoryHistoryDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<InventoryHistoryList> Create(IQueryable<InventoryHistory> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new InventoryHistoryList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<InventoryHistory> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<InventoryHistoryDto>> GetDtos(IQueryable<InventoryHistory> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new InventoryHistoryDto
                {
                    Id = c.Id,
                    InventorySource = c.InventorySource,
                    PricePerUnit = c.PricePerUnit,
                    Stock = c.Stock,
                    ProductId = c.ProductId,
                    CreatedDate = c.CreatedDate,
                    SalesOrderId = c.SalesOrderId,
                    PurchaseOrderId = c.PurchaseOrderId,
                    PurchaseOrderNumber = c.PurchaseOrder != null ? c.PurchaseOrder.OrderNumber : null,
                    SalesOrderNumber = c.SalesOrder != null ? c.SalesOrder.OrderNumber : null,
                }).ToListAsync();
            return entities;
        }
    }
}
