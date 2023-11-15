using POS.Data;
using POS.Data.Resources;
using POS.Helper;
using POS.Repository;
using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllExpenseQuery : IRequest<ExpenseList>
    {
        public ExpenseResource ExpenseResource { get; set; }
    }
}
