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
    public class PaymentCardList : List<PaymentCardDto>
    {
        public PaymentCardList()
        {
        }
        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public PaymentCardList(List<PaymentCardDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<PaymentCardList> Create(IQueryable<PaymentCard> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new PaymentCardList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<PaymentCard> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<PaymentCardDto>> GetDtos(IQueryable<PaymentCard> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new PaymentCardDto
                {
                    Id = c.Id,
                    CardNumber = c.CardNumber,
                    NameOnCard = c.NameOnCard,
                    ExpiryDate = c.ExpiryDate,
                    CardType = c.CardType,
                    CVV = c.CVV,
                    CustomerId = c.Customer.Id,
                    CustomerName = c.Customer.CustomerName,
                    IsPrimary = c.IsPrimary
                }).ToListAsync();
            return entities;
        }
    }
}