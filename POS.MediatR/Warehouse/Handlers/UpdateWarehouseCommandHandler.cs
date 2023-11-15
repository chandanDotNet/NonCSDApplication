using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Warehouse.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Warehouse.Handlers
{
    public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, ServiceResponse<bool>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateWarehouseCommandHandler> _logger;
        public UpdateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateWarehouseCommandHandler> logger
            )
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _warehouseRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
             .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Warehouse Already Exist.");
                return ServiceResponse<bool>.Return409("Warehouse Already Exist.");
            }
            entityExist = await _warehouseRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            var addEntity = _mapper.Map(request, entityExist);
            _warehouseRepository.Update(addEntity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
