using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Product.Handler
{
    public class ProductByWarehouseCommandHandler : IRequestHandler<ProductByWarehouseCommand, ServiceResponse<List<ProductShortDto>>>
    {

        private readonly IWarehouseInventoryRepository _warehouseInventoryRepository;

        public ProductByWarehouseCommandHandler(IWarehouseInventoryRepository warehouseInventoryRepository,
            IMapper mapper)
        {
            _warehouseInventoryRepository = warehouseInventoryRepository;

        }

        public async Task<ServiceResponse<List<ProductShortDto>>> Handle(ProductByWarehouseCommand request, CancellationToken cancellationToken)
        {
            var products = await _warehouseInventoryRepository.All
                .Include(c => c.Product)
                   .ThenInclude(c=>c.Unit)
                .GroupBy(  c=> new { Id= c.Product.Id, Name= c.Product.Name, UnitName= c.Product.Unit.Name, UnitCode= c.Product.Unit.Code })
                                .Select(c => new ProductShortDto
                                {
                                    Id = c.Key.Id,
                                    Name = c.Key.Name,
                                    UnitName=c.Key.UnitName,
                                    UnitCode=c.Key.UnitCode,
                                    Stock = c.Sum(d=>d.Stock)
                                }).ToListAsync();
            return ServiceResponse<List<ProductShortDto>>.ReturnResultWith200(products);
        }
    }
}
