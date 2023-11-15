using MediatR;
using POS.MediatR.PurchaseOrderPayment.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrderPayment.Handler
{
    public class GetAllPurchaseOrderPaymentsReportCommandHandler : IRequestHandler<GetAllPurchaseOrderPaymentsReportCommand, PurchaseOrderPaymentList>
    {
        private readonly IPurchaseOrderPaymentRepository _purchaseOrderPaymentRepository;

        public GetAllPurchaseOrderPaymentsReportCommandHandler(IPurchaseOrderPaymentRepository purchaseOrderPaymentRepository)
        {
            _purchaseOrderPaymentRepository = purchaseOrderPaymentRepository;
        }

        public async Task<PurchaseOrderPaymentList> Handle(GetAllPurchaseOrderPaymentsReportCommand request, CancellationToken cancellationToken)
        {
            return await  _purchaseOrderPaymentRepository.GetAllPurchaseOrderPayments(request.PurchaseOrderResource);
        }
    }
}
