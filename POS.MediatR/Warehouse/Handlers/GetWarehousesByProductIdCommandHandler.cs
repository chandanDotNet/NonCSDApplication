using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Warehouse.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Warehouse.Handlers
{
    public class GetWarehousesByProductIdCommandHandler : IRequestHandler<GetWarehousesByProductIdCommand, ServiceResponse<List<WarehouseUnitDto>>>
    {

        private readonly IWarehouseInventoryRepository _warehouseInventoryRepository;

        public GetWarehousesByProductIdCommandHandler(IWarehouseInventoryRepository warehouseInventoryRepository
           )
        {
            _warehouseInventoryRepository = warehouseInventoryRepository;

        }

        public async Task<ServiceResponse<List<WarehouseUnitDto>>> Handle(GetWarehousesByProductIdCommand request, CancellationToken cancellationToken)
        {
            var products = await _warehouseInventoryRepository.All
                .Include(c => c.Product)
                   .ThenInclude(c => c.Unit)
                .Include(c => c.Warehouse)
                 .Where(c => c.ProductId == request.ProductId)
                .Select(c => new WarehouseUnitDto
                {
                    Id = c.Id,
                    Name=c.Warehouse.Name,
                    Stock=c.Stock,
                    UnitName=c.Product.Unit.Name,
                    UnitCode=c.Product.Unit.Code
                })
                .ToListAsync();
            return ServiceResponse<List<WarehouseUnitDto>>.ReturnResultWith200(products);
        }
    }
}
