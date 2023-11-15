using POS.Data.Resources;
using POS.Repository;
using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllPurchaseOrderQuery : IRequest<PurchaseOrderList>
    {
        public PurchaseOrderResource PurchaseOrderResource { get; set; }
    }
}
