using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetNewPurchaseOrderNumberQueryHandler
        : IRequestHandler<GetNewPurchaseOrderNumberQuery, string>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetNewPurchaseOrderNumberQueryHandler(IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }
        public async Task<string> Handle(GetNewPurchaseOrderNumberQuery request, CancellationToken cancellationToken)
        {
            var lastPurchaseOrder = await _purchaseOrderRepository.All.Where(c=> c.IsPurchaseOrderRequest != request.isPurchaseOrder)
                .OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
            if (lastPurchaseOrder == null)
            {
                if (request.isPurchaseOrder)
                {
                    return "PO#00001";
                }
                else
                {
                    return "POR#00001";
                }
            }

            var lastPONumber = lastPurchaseOrder.OrderNumber;
            var poId = Regex.Match(lastPONumber, @"\d+").Value;
            var isNumber = int.TryParse(poId, out int poNumber);
            if (isNumber)
            {
                var newPoId = lastPONumber.Replace(poNumber.ToString(), "");
                return $"{newPoId}{poNumber + 1}";
            }
            else {
                return $"{lastPONumber}#00001";
            }
        }
    }
}
