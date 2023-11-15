using AutoMapper;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.PurchaseOrder.Commands;
using POS.Data.Dto;

namespace POS.MediatR.PurchaseOrder.Handlers
{
    public class GetPurchaseOrderRecentDeliveryScheduleQueryHandler
        : IRequestHandler<GetPurchaseOrderRecentDeliveryScheduleQuery, List<PurchaseOrderRecentDeliverySchedule>>
    {

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ILogger<GetPurchaseOrderRecentDeliveryScheduleQueryHandler> _logger;


        public GetPurchaseOrderRecentDeliveryScheduleQueryHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IMapper mapper,
            ILogger<GetPurchaseOrderRecentDeliveryScheduleQueryHandler> logger
          )
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _logger = logger;

        }

        public async Task<List<PurchaseOrderRecentDeliverySchedule>> Handle(GetPurchaseOrderRecentDeliveryScheduleQuery request, CancellationToken cancellationToken)
        {
            var entities = await _purchaseOrderRepository
                .AllIncluding(c => c.Supplier, c => c.PurchaseOrderItems)
                .OrderByDescending(c => c.DeliveryDate)
                .Take(10)
                .Select(c => new PurchaseOrderRecentDeliverySchedule
                {
                    PurchaseOrderId = c.Id,
                    PurchaseOrderNumber = c.OrderNumber,
                    ExpectedDispatchDate = c.DeliveryDate,
                    SupplierName = c.Supplier.SupplierName,
                    SupplierId = c.SupplierId,
                    TotalQuantity = c.PurchaseOrderItems.Sum(c => c.Quantity),
                }).ToListAsync();

            return entities;
        }
    }
}
