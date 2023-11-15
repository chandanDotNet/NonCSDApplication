using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
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
    public class GetSaleOrderProfitLossCommandHandler : IRequestHandler<GetSaleOrderProfitLossCommand, ProfitLossDto>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;

        public GetSaleOrderProfitLossCommandHandler(
            ISalesOrderRepository salesOrderRepository)
        {
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<ProfitLossDto> Handle(GetSaleOrderProfitLossCommand request, CancellationToken cancellationToken)
        {
            var salesOrders = await _salesOrderRepository.All
                .Where(c => c.CreatedDate >= new DateTime(request.FromDate.Year, request.FromDate.Month, request.FromDate.Day, 0, 0, 1)
                    && c.CreatedDate <= new DateTime(request.ToDate.Year, request.ToDate.Month, request.ToDate.Day, 23, 59, 59))
                .GroupBy(c => 1)
                .Select(cs => new ProfitLossDto
                {
                    Total = cs.Sum(sales => sales.TotalAmount),
                    TotalTax = cs.Sum(sales => sales.TotalTax),
                    TotalDiscount = cs.Sum(sales => sales.TotalDiscount),
                    PaidPayment = cs.Sum(sales => sales.TotalPaidAmount),
                    TotalItem = cs.Count()
                }).FirstOrDefaultAsync();

            if (salesOrders == null)
            {
                return new ProfitLossDto();
            }

            return salesOrders;
        }
    }
}
