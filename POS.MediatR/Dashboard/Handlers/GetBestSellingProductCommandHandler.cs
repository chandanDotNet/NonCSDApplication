using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.MediatR.Dashboard.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Dashboard.Handlers
{
    public class GetBestSellingProductCommandHandler : IRequestHandler<GetBestSellingProductCommand, List<BestSellingProductStatisticDto>>
    {
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;

        public GetBestSellingProductCommandHandler(ISalesOrderItemRepository salesOrderItemRepository)
        {
            _salesOrderItemRepository = salesOrderItemRepository;
        }

        public async Task<List<BestSellingProductStatisticDto>> Handle(GetBestSellingProductCommand request, CancellationToken cancellationToken)
        {
            var bestSellingProductStatistics = await _salesOrderItemRepository.AllIncluding(c => c.SalesOrder, cs => cs.Product)
                .Where(c => c.SalesOrder.CreatedDate.Month == request.Month && c.SalesOrder.CreatedDate.Year == request.Year)
                .GroupBy(c => c.ProductId)
                .Select(cs => new BestSellingProductStatisticDto
                {
                    Name = cs.FirstOrDefault().Product.Name,
                    Count = cs.Sum(item => item.Quantity)
                })
                .OrderByDescending(c => c.Count)
                .Take(10)
                .ToListAsync();
            return bestSellingProductStatistics;
        }
    }
}
