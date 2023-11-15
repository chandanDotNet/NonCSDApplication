using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class PurchaseOrderPaymentList : List<PurchaseOrderPaymentDto>
    {
        public PurchaseOrderPaymentList()
        {

        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PurchaseOrderPaymentList(List<PurchaseOrderPaymentDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<PurchaseOrderPaymentList> Create(IQueryable<PurchaseOrderPayment> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new PurchaseOrderPaymentList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<PurchaseOrderPayment> source)
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

        public async Task<List<PurchaseOrderPaymentDto>> GetDtos(IQueryable<PurchaseOrderPayment> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new PurchaseOrderPaymentDto
                    {
                        Id = cs.Id,
                        PurchaseOrderId = cs.PurchaseOrderId,
                        OrderNumber = cs.PurchaseOrder.OrderNumber,
                        PaymentDate = cs.PaymentDate,
                        ReferenceNumber = cs.ReferenceNumber,
                        Amount = cs.Amount,
                        PaymentMethod = cs.PaymentMethod,
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
              .Select(cs => new PurchaseOrderPaymentDto
              {
                  Id = cs.Id,
                  PurchaseOrderId = cs.PurchaseOrderId,
                  OrderNumber = cs.PurchaseOrder.OrderNumber,
                  PaymentDate = cs.PaymentDate,
                  ReferenceNumber = cs.ReferenceNumber,
                  Amount = cs.Amount,
                  PaymentMethod = cs.PaymentMethod,
                  Note = cs.Note
              })
              .ToListAsync();
                return entities;
            }
        }
    }
}
