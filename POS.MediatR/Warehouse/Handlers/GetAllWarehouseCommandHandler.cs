using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Warehouse.Commands;
using POS.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Warehouse.Handlers
{
    public class GetAllWarehouseCommandHandler : IRequestHandler<GetAllWarehouseCommand, List<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        public GetAllWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public async Task<List<WarehouseDto>> Handle(GetAllWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entities = await _warehouseRepository.All.ProjectTo<WarehouseDto>(_mapper.ConfigurationProvider).ToListAsync();
            return entities;
        }
    }
}
