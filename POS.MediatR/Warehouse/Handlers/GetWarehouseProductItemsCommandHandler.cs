using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class GetWarehouseProductItemsCommandHandler : IRequestHandler<GetWarehouseProductItemsCommand, List<WarehouseInventoryDto>>
    {
        private readonly IWarehouseInventoryRepository _warehouseInventoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWarehouseProductItemsCommandHandler> _logger;
        public GetWarehouseProductItemsCommandHandler(
            IWarehouseInventoryRepository warehouseInventoryRepository,
            IMapper mapper,
            ILogger<GetWarehouseProductItemsCommandHandler> logger
            )
        {
            _warehouseInventoryRepository = warehouseInventoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<WarehouseInventoryDto>> Handle(GetWarehouseProductItemsCommand request, CancellationToken cancellationToken)
        {
            var itemsQuery = _warehouseInventoryRepository.AllIncluding(c => c.Warehouse, cs => cs.Product)
               .Where(c => c.WarehouseId == request.Id);
          
            var items = await itemsQuery
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new WarehouseInventoryDto
                {
                    ProductName = c.Product.Name,
                    Stock = c.Stock,
                    BaseUnit =c.Product.Unit.Name,
                    ProductId = c.ProductId,
                    WarehouseId= c.Warehouse.Id,
                    Id = c.Id,
                    UnitCode =c.Product.Unit.Code
                }).ToListAsync();
            return items;
        }
    }
}
