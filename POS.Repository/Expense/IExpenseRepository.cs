using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        Task<ExpenseList> GetExpenses(ExpenseResource expenseResource);

        Task<ExpenseList> GetExpensesReport(ExpenseResource expenseResource);
    }
}
