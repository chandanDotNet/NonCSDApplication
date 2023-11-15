using MediatR;
using POS.MediatR.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetPurchaseOrderItemsReportCommandHandler : IRequestHandler<GetPurchaseOrderItemsReportCommand, PurchaseOrderItemList>
    {
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;

        public GetPurchaseOrderItemsReportCommandHandler(IPurchaseOrderItemRepository purchaseOrderItemRepository)
        {
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
        }


        public async Task<PurchaseOrderItemList> Handle(GetPurchaseOrderItemsReportCommand request, CancellationToken cancellationToken)
        {
            return await _purchaseOrderItemRepository.GetAllPurchaseOrderItems(request.PurchaseOrderResource);
        }
    }
}
