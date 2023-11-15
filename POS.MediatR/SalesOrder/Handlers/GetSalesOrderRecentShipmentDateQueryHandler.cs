using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Handlers
{
    public class GetSalesOrderRecentShipmentDateQueryHandler
        : IRequestHandler<GetSalesOrderRecentShipmentDateQuery, List<SalesOrderRecentShipmentDate>>
    {

        private readonly ISalesOrderRepository _salesOrderRepository;

        public GetSalesOrderRecentShipmentDateQueryHandler(
            ISalesOrderRepository salesOrderRepository
          )
        {
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<List<SalesOrderRecentShipmentDate>> Handle(GetSalesOrderRecentShipmentDateQuery request, CancellationToken cancellationToken)
        {
            var entities = await _salesOrderRepository
                .AllIncluding(c => c.Customer, cs => cs.SalesOrderItems)
                .OrderByDescending(c => c.DeliveryDate)
                         .Take(10)
                         .Select(c => new SalesOrderRecentShipmentDate
                         {
                             SalesOrderId = c.Id,
                             SalesOrderNumber = c.OrderNumber,
                             ExpectedShipmentDate = c.DeliveryDate,
                             Quantity = c.SalesOrderItems.Sum(c => c.Quantity),
                             CustomerId = c.CustomerId,
                             CustomerName = c.Customer.CustomerName,
                         })
                     .ToListAsync();

            return entities;
        }
    }
}
