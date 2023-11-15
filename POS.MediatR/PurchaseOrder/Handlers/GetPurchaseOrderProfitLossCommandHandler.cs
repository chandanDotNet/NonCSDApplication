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
using POS.MediatR.PurchaseOrder.Commands;
using POS.MediatR.SalesOrder.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrder.Handlers
{
    public class GetPurchaseOrderProfitLossCommandHandler : IRequestHandler<GetPurchaseOrderProfitLossCommand, ProfitLossDto>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        public GetPurchaseOrderProfitLossCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ProfitLossDto> Handle(GetPurchaseOrderProfitLossCommand request, CancellationToken cancellationToken)
        {
            var profitLoss = await _purchaseOrderRepository.All
                .Where(c => c.CreatedDate >= new DateTime(request.FromDate.Year, request.FromDate.Month, request.FromDate.Day, 0, 0, 1)
                    && c.CreatedDate <= new DateTime(request.ToDate.Year, request.ToDate.Month, request.ToDate.Day, 23, 59, 59))
                .GroupBy(c => 1)
                .Select(cs => new ProfitLossDto
                {
                    Total = cs.Sum(purchase => purchase.TotalAmount),
                    TotalTax = cs.Sum(purchase => purchase.TotalTax),
                    TotalDiscount = cs.Sum(purchase => purchase.TotalDiscount),
                    PaidPayment = cs.Sum(purchase => purchase.TotalPaidAmount),
                    TotalItem = cs.Count()
                }).FirstOrDefaultAsync();

            if (profitLoss == null)
            {
                return new ProfitLossDto();
            }

            return profitLoss;
        }
    }
}
