using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetNewSupplierQueryHandler : IRequestHandler<GetNewSupplierQuery, List<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly PathHelper _pathHelper;
        public GetNewSupplierQueryHandler(ISupplierRepository supplierRepository,
            PathHelper pathHelper
            )
        {
            _supplierRepository = supplierRepository;
            _pathHelper = pathHelper;
        }

        public async Task<List<SupplierDto>> Handle(GetNewSupplierQuery request, CancellationToken cancellationToken)
        {
            var entities = await _supplierRepository.All.Where(cs => cs.IsVarified && !cs.IsDeleted)
                .OrderByDescending(c => c.CreatedDate).Take(10)
                .Select(c => new SupplierDto
                {
                    Id = c.Id,
                    SupplierName = c.SupplierName,
                    Url = string.IsNullOrWhiteSpace(c.Url) ? _pathHelper.NoImageFound : Path.Combine(_pathHelper.SupplierImagePath, c.Url),
                    Description = c.Description
                }).ToListAsync();
            return entities;
        }
    }
}
