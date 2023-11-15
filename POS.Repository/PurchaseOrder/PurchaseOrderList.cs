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
    public class PurchaseOrderList : List<PurchaseOrderDto>
    {
        public PurchaseOrderList()
        {

        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PurchaseOrderList(List<PurchaseOrderDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<PurchaseOrderList> Create(IQueryable<PurchaseOrder> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new PurchaseOrderList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<PurchaseOrder> source)
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

        public async Task<List<PurchaseOrderDto>> GetDtos(IQueryable<PurchaseOrder> source, int skip, int pageSize)
        {
            if(pageSize == 0) { 
            var entities = await source
                .AsNoTracking()
                .Select(cs => new PurchaseOrderDto
                {
                    Id = cs.Id,
                    POCreatedDate = cs.POCreatedDate,
                    OrderNumber = cs.OrderNumber,
                    SupplierId = cs.SupplierId,
                    TotalAmount = cs.TotalAmount,
                    TotalDiscount = cs.TotalDiscount,
                    DeliveryStatus = cs.DeliveryStatus,
                    DeliveryDate = cs.DeliveryDate,
                    TotalTax = cs.TotalTax,
                    SupplierName = cs.Supplier.SupplierName,
                    Status = cs.Status,
                    PaymentStatus = cs.PaymentStatus,
                    TotalPaidAmount = cs.TotalPaidAmount,
                    TermAndCondition = cs.TermAndCondition,
                    IsPurchaseOrderRequest = cs.IsPurchaseOrderRequest,
                    Note = cs.Note
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
             .Select(cs => new PurchaseOrderDto
             {
                 Id = cs.Id,
                 POCreatedDate = cs.POCreatedDate,
                 OrderNumber = cs.OrderNumber,
                 SupplierId = cs.SupplierId,
                 TotalAmount = cs.TotalAmount,
                 TotalDiscount = cs.TotalDiscount,
                 DeliveryStatus = cs.DeliveryStatus,
                 DeliveryDate = cs.DeliveryDate,
                 TotalTax = cs.TotalTax,
                 SupplierName = cs.Supplier.SupplierName,
                 Status = cs.Status,
                 PaymentStatus = cs.PaymentStatus,
                 TotalPaidAmount = cs.TotalPaidAmount,
                 IsPurchaseOrderRequest = cs.IsPurchaseOrderRequest,
                 TermAndCondition = cs.TermAndCondition,
                 Note = cs.Note
             })
             .ToListAsync();
                return entities;
            }
        }
    }
}
