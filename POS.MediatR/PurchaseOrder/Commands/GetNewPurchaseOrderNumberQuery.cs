using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetNewPurchaseOrderNumberQuery : IRequest<string>
    {
        public bool isPurchaseOrder { get; set; }
    }
}
