using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class SupplierPaymentList : List<SupplierPaymentDto>
    {
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public SupplierPaymentList()
        {
        }

        public SupplierPaymentList(List<SupplierPaymentDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<SupplierPaymentList> Create(IQueryable<IGrouping<Guid, PurchaseOrder>> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var dtoPageList = new SupplierPaymentList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<IGrouping<Guid, PurchaseOrder>> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<SupplierPaymentDto>> GetDtos(IQueryable<IGrouping<Guid, PurchaseOrder>> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
            .AsNoTracking()
            .Select(c => new SupplierPaymentDto
            {
                Id = c.Key,
                SupplierName = c.FirstOrDefault().Supplier.SupplierName,
                TotalAmount = c.Sum(cs => cs.TotalAmount),
                TotalPaidAmount = c.Sum(cs => cs.TotalPaidAmount),
                TotalPendingAmount = c.Sum(cs => cs.TotalAmount - cs.TotalPaidAmount)
            }).ToListAsync();
                return entities;

            }
            else
            {
                var entities = await source
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .Select(c => new SupplierPaymentDto
            {
                Id = c.Key,
                SupplierName = c.FirstOrDefault().Supplier.SupplierName,
                TotalAmount = c.Sum(cs => cs.TotalAmount),
                TotalPaidAmount = c.Sum(cs => cs.TotalPaidAmount),
                TotalPendingAmount = c.Sum(cs => cs.TotalAmount - cs.TotalPaidAmount)
            }).ToListAsync();
                return entities;
            }

        }
    }
}
