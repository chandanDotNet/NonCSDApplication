using MediatR;
using POS.MediatR.SalesOrderPayment.Command;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrderPayment.Handler
{
    public class GetAllSalesOrderPaymentsReportCommandHandler : IRequestHandler<GetAllSalesOrderPaymentsReportCommand, SaleOrderPaymentList>
    {
        private readonly ISalesOrderPaymentRepository _salesOrderPaymentRepository;

        public GetAllSalesOrderPaymentsReportCommandHandler(
            ISalesOrderPaymentRepository salesOrderPaymentRepository)
        {
            _salesOrderPaymentRepository = salesOrderPaymentRepository;
        }

        public async Task<SaleOrderPaymentList> Handle(GetAllSalesOrderPaymentsReportCommand request, CancellationToken cancellationToken)
        {
            return await _salesOrderPaymentRepository.GetAllSaleOrderPayments(request.SalesOrderResource);
        }
    }
}
