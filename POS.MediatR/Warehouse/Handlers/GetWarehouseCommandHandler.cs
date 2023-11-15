using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Warehouse.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Warehouse.Handlers
{
    public class GetWarehouseCommandHandler : IRequestHandler<GetWarehouseCommand, ServiceResponse<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWarehouseCommandHandler> _logger;
        public GetWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IMapper mapper,
            ILogger<GetWarehouseCommandHandler> logger
            )
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<WarehouseDto>> Handle(GetWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _warehouseRepository.FindBy(c => c.Id == request.Id)
              .ProjectTo<WarehouseDto>(_mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Unit is not available");
                return ServiceResponse<WarehouseDto>.Return404();
            }
            return ServiceResponse<WarehouseDto>.ReturnResultWith200(entityDto);
        }
    }
}
