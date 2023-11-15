using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllPurchaseOrderQueryHandler : IRequestHandler<GetAllPurchaseOrderQuery, PurchaseOrderList>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetAllPurchaseOrderQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<PurchaseOrderList> Handle(GetAllPurchaseOrderQuery request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderRepository.GetAllPurchaseOrders(request.PurchaseOrderResource);
        }
    }
}
