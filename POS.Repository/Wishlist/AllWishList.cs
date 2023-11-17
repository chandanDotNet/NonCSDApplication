using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class AllWishList : List<WishlistDto>
    {

        public IMapper _mapper { get; set; }        
        public AllWishList(IMapper mapper)
        {
            _mapper = mapper;           
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public AllWishList(List<WishlistDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<AllWishList> Create(IQueryable<Wishlist> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            pageSize = GetAllData(count, pageSize);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new AllWishList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Wishlist> source)
        {
            return await source.AsNoTracking().CountAsync();
        }
        public int GetAllData(int totalCount, int pageSize)
        {
            if (pageSize == 0)
            {
                pageSize = totalCount;
            }
            return pageSize;
        }

        public async Task<List<WishlistDto>> GetDtos(IQueryable<Wishlist> source, int skip, int pageSize)
        {
           
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new WishlistDto
                {
                    Id = c.Id,                   
                    CustomerId = c.CustomerId,
                    ProductId = c.ProductId,
                    Product= _mapper.Map<ProductDto>(c.Product),                    
                })
                .ToListAsync();
            return entities;
        }



    }
}
