using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using POS.Data.Entities;

namespace POS.MediatR.Handlers
{
    public class DashboardStaticaticsQueryHandler : IRequestHandler<DashboardStaticaticsQuery, DashboardStatics>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IPurchaseOrderItemRepository _purchaseOrderItemRepository;

        public DashboardStaticaticsQueryHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            ISalesOrderItemRepository salesOrderItemRepository,
            ISalesOrderRepository salesOrderRepository,
            IPurchaseOrderItemRepository purchaseOrderItemRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
            _purchaseOrderItemRepository = purchaseOrderItemRepository;
        }
        public async Task<DashboardStatics> Handle(DashboardStaticaticsQuery request, CancellationToken cancellationToken)
        {
            var dashboardStatics = new DashboardStatics();
            dashboardStatics.TotalPurchase = await _purchaseOrderRepository.All.SumAsync(c => c.TotalAmount);
            dashboardStatics.TotalSales = await _salesOrderRepository.All.SumAsync(c => c.TotalAmount);
            dashboardStatics.TotalSalesReturn = await _salesOrderItemRepository.All
                .Where(c => c.Status == PurchaseSaleItemStatusEnum.Return)
                .SumAsync(c => (c.UnitPrice * c.Quantity) + c.TaxValue - c.Discount);
            dashboardStatics.TotalPurchaseReturn = await _purchaseOrderItemRepository.All
                .Where(c => c.Status == PurchaseSaleItemStatusEnum.Return)
                .SumAsync(c => (c.UnitPrice * c.Quantity) + c.TaxValue - c.Discount);
            return dashboardStatics;
        }
    }
}
