using MediatR;
using POS.Data.Resources;
using POS.Repository;

namespace POS.MediatR.Commands
{
    public class GetAllExpenseReportQuery : IRequest<ExpenseList>
    {
        public ExpenseResource ExpenseResource { get; set; }
    }
}
