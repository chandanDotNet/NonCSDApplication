using MediatR;
using POS.Data.Resources;
using POS.Repository;

namespace POS.MediatR.PurchaseOrderPayment.Command
{
    public class GetAllPurchaseOrderPaymentsReportCommand : IRequest<PurchaseOrderPaymentList>
    {
        public PurchaseOrderResource PurchaseOrderResource { get; set; }
    }
}
