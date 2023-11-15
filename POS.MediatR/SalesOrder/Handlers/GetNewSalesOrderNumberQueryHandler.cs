using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetNewSalesOrderNumberQueryHandler : IRequestHandler<GetNewSalesOrderNumberCommand, string>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        public GetNewSalesOrderNumberQueryHandler(ISalesOrderRepository salesOrderRepository)
        {
            _salesOrderRepository = salesOrderRepository;
        }
        public async Task<string> Handle(GetNewSalesOrderNumberCommand request, CancellationToken cancellationToken)
        {
            var lastSalesOrder = await _salesOrderRepository.All.OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
            if (lastSalesOrder == null)
            {
                return "SO#00001";
            }

            var lastSONumber = lastSalesOrder.OrderNumber;
            var soId = Regex.Match(lastSONumber, @"\d+").Value;
            var isNumber = int.TryParse(soId, out int soNumber);
            if (isNumber)
            {
                var newSoId = lastSONumber.Replace(soNumber.ToString(), "");
                return $"{newSoId}{soNumber + 1}";
            }
            else
            {
                return $"{lastSONumber}#00001";
            }
        }
    }
}
