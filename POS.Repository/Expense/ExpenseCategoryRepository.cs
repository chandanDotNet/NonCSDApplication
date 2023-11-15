using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Domain;

namespace POS.Repository
{
   public class ExpenseCategoryRepository : GenericRepository<ExpenseCategory, POSDbContext>,
          IExpenseCategoryRepository
    {
        public ExpenseCategoryRepository(
            IUnitOfWork<POSDbContext> uow
            ) : base(uow)
        {
        }
    }
}