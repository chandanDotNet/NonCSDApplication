using AutoMapper;
using POS.Data;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace POS.MediatR
{
    public class GetAllExpenseQueryHandler : IRequestHandler<GetAllExpenseQuery, ExpenseList>
    {

        private readonly IExpenseRepository _expenseRepository;

        public GetAllExpenseQueryHandler(
            IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<ExpenseList> Handle(GetAllExpenseQuery request, CancellationToken cancellationToken)
        {
            return await _expenseRepository.GetExpenses(request.ExpenseResource);
        }
    }
}
