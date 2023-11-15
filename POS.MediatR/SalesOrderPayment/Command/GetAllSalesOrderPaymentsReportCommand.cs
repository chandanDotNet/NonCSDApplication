using MediatR;
using POS.Data.Resources;
using POS.Repository;

namespace POS.MediatR.SalesOrderPayment.Command
{
    public class GetAllSalesOrderPaymentsReportCommand : IRequest<SaleOrderPaymentList>
    {
        public SalesOrderResource SalesOrderResource { get; set; }
    }
}
