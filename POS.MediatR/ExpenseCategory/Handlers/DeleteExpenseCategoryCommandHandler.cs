using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class DeleteExpenseCategoryCommandHandler
        : IRequestHandler<DeleteExpenseCategoryCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteExpenseCategoryCommandHandler> _logger;
        public DeleteExpenseCategoryCommandHandler(
           IExpenseCategoryRepository expenseCategoryRepository,
            IExpenseRepository expenseRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteExpenseCategoryCommandHandler> logger
            )
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _expenseRepository = expenseRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _expenseCategoryRepository.FindBy(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (existingEntity == null)
            {
                _logger.LogError("Expense Category does not Exist");
                return ServiceResponse<bool>.Return409("Expense Category does not  Exist.");
            }
            var exitingExpense = _expenseRepository.AllIncluding(c => c.ExpenseCategory).Any(c => c.ExpenseCategory.Id == existingEntity.Id);
            if (exitingExpense)
            {
                _logger.LogError("Expense Category can not be Deleted because it is use in Expense");
                return ServiceResponse<bool>.Return409("Expense Category can not be Deleted because it is use in Expense.");
            }

            _expenseCategoryRepository.Delete(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Expense Category.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
