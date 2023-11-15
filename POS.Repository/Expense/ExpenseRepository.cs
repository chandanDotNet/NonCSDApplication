using AutoMapper;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace POS.Repository
{
    public class ExpenseRepository : GenericRepository<Expense, POSDbContext>,
            IExpenseRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IMapper _mapper;
        public ExpenseRepository(
            IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService,
            IMapper mapper
            ) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
            _mapper = mapper;
        }

        public async Task<ExpenseList> GetExpenses(ExpenseResource expenseResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.ExpenseBy, cs => cs.ExpenseCategory).ApplySort(expenseResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ExpenseDto, Expense>());

            if (!string.IsNullOrEmpty(expenseResource.Reference))
            {
                // trim & ignore casing
                var referenceWhereClause = expenseResource.Reference
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Reference, $"{referenceWhereClause}%"));
            }

            if (expenseResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseDate >= new DateTime(expenseResource.FromDate.Value.Year, expenseResource.FromDate.Value.Month, expenseResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (expenseResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseDate <= new DateTime(expenseResource.ToDate.Value.Year, expenseResource.ToDate.Value.Month, expenseResource.ToDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrEmpty(expenseResource.Description))
            {
                // trim & ignore casing
                var descriptionWhereClause = expenseResource.Description
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Description, $"{descriptionWhereClause}%"));
            }

            if (expenseResource.ExpenseCategoryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseCategoryId == expenseResource.ExpenseCategoryId);
            }

            if (expenseResource.ExpenseById.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseById == expenseResource.ExpenseById);
            }

            return await new ExpenseList(_mapper).Create(collectionBeforePaging,
                expenseResource.Skip,
                expenseResource.PageSize);
        }


        public async Task<ExpenseList> GetExpensesReport(ExpenseResource expenseResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.ExpenseBy, cs => cs.ExpenseCategory).ApplySort(expenseResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<ExpenseDto, Expense>());

            if (!string.IsNullOrEmpty(expenseResource.Reference))
            {
                // trim & ignore casing
                var referenceWhereClause = expenseResource.Reference
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Reference, $"{referenceWhereClause}%"));
            }

            if (expenseResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseDate >= new DateTime(expenseResource.FromDate.Value.Year, expenseResource.FromDate.Value.Month, expenseResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (expenseResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseDate <= new DateTime(expenseResource.ToDate.Value.Year, expenseResource.ToDate.Value.Month, expenseResource.ToDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrEmpty(expenseResource.Description))
            {
                // trim & ignore casing
                var descriptionWhereClause = expenseResource.Description
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.Description, $"{descriptionWhereClause}%"));
            }

            if (expenseResource.ExpenseCategoryId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseCategoryId == expenseResource.ExpenseCategoryId);
            }

            if (expenseResource.ExpenseById.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ExpenseById == expenseResource.ExpenseById);
            }

            return await new ExpenseList(_mapper).Create(collectionBeforePaging,
                0,
                0);
        }
    }
}