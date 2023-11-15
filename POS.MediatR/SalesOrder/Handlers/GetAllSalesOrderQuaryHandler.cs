using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllSalesOrderQuaryHandler : IRequestHandler<GetAllSalesOrderCommand, SalesOrderList>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        public GetAllSalesOrderQuaryHandler(ISalesOrderRepository salesOrderRepository)
        {
            _salesOrderRepository = salesOrderRepository;
        }

        public Task<SalesOrderList> Handle(GetAllSalesOrderCommand request, CancellationToken cancellationToken)
        {
            return _salesOrderRepository.GetAllSalesOrders(request.SalesOrderResource);
        }
    }
}
