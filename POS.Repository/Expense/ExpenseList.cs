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
    public class ExpenseList : List<ExpenseDto>
    {
        IMapper _mapper;
        public ExpenseList(IMapper mapper)
        {
            _mapper = mapper;
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public decimal TotalAmount { get; set; }

        public ExpenseList(List<ExpenseDto> items, int count, int skip, int pageSize, decimal totalAmount)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalAmount = totalAmount;
            AddRange(items);
        }

        public async Task<ExpenseList> Create(IQueryable<Expense> source, int skip, int pageSize)
        {

            var dtoList = await GetDtos(source, skip, pageSize);
            var count = pageSize == 0 || dtoList.Count() == 0 ? dtoList.Count() : await GetCount(source);
            var totalAmount = await GetTotalAmount(source);
            var dtoPageList = new ExpenseList(dtoList, count, skip, pageSize, totalAmount);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Expense> source)
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

        public async Task<decimal> GetTotalAmount(IQueryable<Expense> source)
        {
            try
            {
                return await source.AsNoTracking().SumAsync(c => c.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ExpenseDto>> GetDtos(IQueryable<Expense> source, int skip, int pageSize)
        {
            if (pageSize == 0)
            {
                var entities = await source
                    .AsNoTracking()
                    .Select(cs => new ExpenseDto
                    {
                        Id = cs.Id,
                        Amount = cs.Amount,
                        Description = cs.Description,
                        ExpenseBy = cs.ExpenseBy !=null? _mapper.Map<UserDto>(cs.ExpenseBy): null,
                        ExpenseById = cs.ExpenseById,
                        ExpenseCategory = _mapper.Map<ExpenseCategoryDto>(cs.ExpenseCategory),
                        ExpenseCategoryId = cs.ExpenseCategoryId,
                        Reference = cs.Reference,
                        CreatedDate = cs.CreatedDate,
                        ExpenseDate = cs.ExpenseDate,
                        ReceiptName = cs.ReceiptName
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
             .Select(cs => new ExpenseDto
             {
                 Id = cs.Id,
                 Amount = cs.Amount,
                 Description = cs.Description,
                 ExpenseBy = _mapper.Map<UserDto>(cs.ExpenseBy),
                 ExpenseById = cs.ExpenseById,
                 ExpenseCategory = _mapper.Map<ExpenseCategoryDto>(cs.ExpenseCategory),
                 ExpenseCategoryId = cs.ExpenseCategoryId,
                 Reference = cs.Reference,
                 CreatedDate = cs.CreatedDate,
                 ExpenseDate = cs.ExpenseDate,
                 ReceiptName = cs.ReceiptName
             })
             .ToListAsync();
                return entities;
            }
        }
    }
}
