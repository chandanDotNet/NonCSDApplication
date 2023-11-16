using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class CustomerAddressList : List<CustomerAddressDto>
    {
        public CustomerAddressList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public CustomerAddressList(List<CustomerAddressDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CustomerAddressList> Create(IQueryable<CustomerAddress> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new CustomerAddressList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<CustomerAddress> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<CustomerAddressDto>> GetDtos(IQueryable<CustomerAddress> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new CustomerAddressDto
                {
                    Id = c.Id,
                    HouseNo = c.HouseNo,
                    LandMark = c.LandMark,
                    StreetDetails = c.StreetDetails,
                    Type = c.Type,
                    CustomerId = c.Customer.Id,
                    CustomerName = c.Customer.CustomerName,
                    IsPrimary = c.IsPrimary
                }).ToListAsync();
            return entities;
        }
    }
}