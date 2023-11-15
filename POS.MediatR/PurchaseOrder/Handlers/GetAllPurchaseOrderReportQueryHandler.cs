using MediatR;
using POS.MediatR.PurchaseOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrder.Handlers
{
    public class GetAllPurchaseOrderReportQueryHandler : IRequestHandler<GetAllPurchaseOrderReportQuery, PurchaseOrderList>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetAllPurchaseOrderReportQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<PurchaseOrderList> Handle(GetAllPurchaseOrderReportQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderRepository.GetAllPurchaseOrdersReport(request.PurchaseOrderResource);
        }
    }
}