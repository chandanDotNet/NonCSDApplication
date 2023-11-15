using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class SearchSupplierQueryHandler : IRequestHandler<SearchSupplierQuery, List<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;


        public SearchSupplierQueryHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<List<SupplierDto>> Handle(SearchSupplierQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepository.All.Where(a => EF.Functions.Like(a.SupplierName, $"{request.SearchQuery}%"))
                .Take(request.PageSize)
                .Select(c => new SupplierDto
                {
                    Id = c.Id,
                    SupplierName = c.SupplierName
                }).ToListAsync();
            return suppliers;
        }
    }
}
