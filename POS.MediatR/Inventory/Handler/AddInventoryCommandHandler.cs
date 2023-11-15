using AutoMapper;
using MediatR;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Inventory.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Inventory.Handler
{
    internal class AddInventoryCommandHandler : IRequestHandler<AddInventoryCommand, ServiceResponse<bool>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<POSDbContext> _uow;

        public AddInventoryCommandHandler(IInventoryRepository inventoryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow)
        {
            _inventoryRepository = inventoryRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResponse<bool>> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = _mapper.Map<InventoryDto>(request);
            inventory.InventorySource = InventorySourceEnum.Direct;
            inventory = _inventoryRepository.ConvertStockAndPriceToBaseUnit(inventory);
            await _inventoryRepository.AddInventory(inventory);
            await _inventoryRepository.AddWarehouseInventory(inventory);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
