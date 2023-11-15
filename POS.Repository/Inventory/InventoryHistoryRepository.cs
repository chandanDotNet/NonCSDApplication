using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class InventoryHistoryRepository
        : GenericRepository<InventoryHistory, POSDbContext>, IInventoryHistoryRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public InventoryHistoryRepository(IUnitOfWork<POSDbContext> uow,
           IPropertyMappingService propertyMappingService)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<InventoryHistoryList> GetInventoryHistories(InventoryHistoryResource inventoryHistoryResource)
        {
            var collectionBeforePaging =
               AllIncluding(c => c.Product, u => u.Product.Unit, c => c.SalesOrder, c => c.PurchaseOrder).ApplySort(inventoryHistoryResource.OrderBy,
               _propertyMappingService.GetPropertyMapping<InventoryHistoryDto, InventoryHistory>());

            collectionBeforePaging = collectionBeforePaging
                .Where(a => (!a.PurchaseOrderId.HasValue || !a.PurchaseOrder.IsDeleted) 
                    && (!a.SalesOrderId.HasValue || !a.SalesOrder.IsDeleted)
                    && !a.Product.IsDeleted && a.ProductId == inventoryHistoryResource.ProductId);
            var inventoryHistoryList = new InventoryHistoryList();

            return await inventoryHistoryList.Create(collectionBeforePaging, inventoryHistoryResource.Skip, inventoryHistoryResource.PageSize);
        }
    }
}
