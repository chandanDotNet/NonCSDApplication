using MediatR;
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
    public class GetAllInventoryCommandHandler : IRequestHandler<GetAllInventoryCommand, InventoryList>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public GetAllInventoryCommandHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<InventoryList> Handle(GetAllInventoryCommand request, CancellationToken cancellationToken)
        {
            return await _inventoryRepository.GetInventories(request.InventoryResource);
        }
    }
}
