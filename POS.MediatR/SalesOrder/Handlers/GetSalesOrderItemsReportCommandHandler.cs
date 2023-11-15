using MediatR;
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
    public class GetSalesOrderItemsReportCommandHandler
        : IRequestHandler<GetSalesOrderItemsReportCommand, SalesOrderItemList>
    {
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;

        public GetSalesOrderItemsReportCommandHandler(ISalesOrderItemRepository salesOrderItemRepository)
        {
            _salesOrderItemRepository = salesOrderItemRepository;
        }


        public async Task<SalesOrderItemList> Handle(GetSalesOrderItemsReportCommand request, CancellationToken cancellationToken)
        {
            return await _salesOrderItemRepository.GetAllSalesOrderItems(request.SalesOrderResource);
        }
    }
}
