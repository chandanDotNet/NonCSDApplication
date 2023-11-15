using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Warehouse.Commands;
using POS.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Warehouse.Handlers
{
    public class AddWarehouseCommandHandler : IRequestHandler<AddWarehouseCommand, ServiceResponse<WarehouseDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddWarehouseCommandHandler> _logger;
        public AddWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddWarehouseCommandHandler> logger
            )
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<WarehouseDto>> Handle(AddWarehouseCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _warehouseRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Warehouse name Already Exist");
                return ServiceResponse<WarehouseDto>.Return409("Warehouse name  Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.Warehouse>(request);
            entity.Id = Guid.NewGuid();
            _warehouseRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<WarehouseDto>.Return500();
            }
            return ServiceResponse<WarehouseDto>.ReturnResultWith200(_mapper.Map<WarehouseDto>(entity));
        }
    }
}
