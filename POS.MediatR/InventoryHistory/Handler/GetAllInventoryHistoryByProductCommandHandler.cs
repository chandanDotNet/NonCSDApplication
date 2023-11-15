using MediatR;
using POS.MediatR.Inventory.Command;
using POS.MediatR.InventoryHistory.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.InventoryHistory.Handler
{
    public class GetAllInventoryHistoryByProductCommandHandler : IRequestHandler<GetAllInventoryHistoryByProductCommand, InventoryHistoryList>
    {
        private readonly IInventoryHistoryRepository _inventoryHistoryRepository;

        public GetAllInventoryHistoryByProductCommandHandler(IInventoryHistoryRepository inventoryHistoryRepository)
        {
            _inventoryHistoryRepository = inventoryHistoryRepository;
        }

        public async Task<InventoryHistoryList> Handle(GetAllInventoryHistoryByProductCommand request, CancellationToken cancellationToken)
        {
            return await _inventoryHistoryRepository.GetInventoryHistories(request.InventoryHistoryResource);
        }
    }
}
