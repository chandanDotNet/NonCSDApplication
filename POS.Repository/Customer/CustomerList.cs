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
    public class CustomerList : List<CustomerDto>
    {
        public IMapper _mapper { get; set; }
        public CustomerList(IMapper mapper)
        {
            _mapper = mapper;
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public CustomerList(List<CustomerDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<CustomerList> Create(IQueryable<Customer> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new CustomerList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Customer> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<CustomerDto>> GetDtos(IQueryable<Customer> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName,
                    Email = c.Email,
                    ContactPerson = c.ContactPerson,
                    MobileNo = c.MobileNo,
                    Website = c.Website,
                    IsWalkIn = c.IsWalkIn,
                    IsVarified = c.IsVarified,
                    OTP = c.OTP
                })
                .ToListAsync();
            return entities;
        }
    }
}
