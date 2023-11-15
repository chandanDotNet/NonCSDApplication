using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class SalesOrderItemRepository
     : GenericRepository<SalesOrderItem, POSDbContext>, ISalesOrderItemRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public SalesOrderItemRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<SalesOrderItemList> GetAllSalesOrderItems(SalesOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.SalesOrder, c => c.Product).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<SalesOrderItemDto, SalesOrderItem>());



            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.ProductName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Product.Name == purchaseOrderResource.ProductName);
            }

            if (purchaseOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.ProductId == purchaseOrderResource.ProductId);
            }

            if (purchaseOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesOrder.SOCreatedDate >= new DateTime(purchaseOrderResource.FromDate.Value.Year, purchaseOrderResource.FromDate.Value.Month, purchaseOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (purchaseOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesOrder.SOCreatedDate <= new DateTime(purchaseOrderResource.ToDate.Value.Year, purchaseOrderResource.ToDate.Value.Month, purchaseOrderResource.ToDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.CustomerName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesOrder.Customer.CustomerName == purchaseOrderResource.CustomerName.GetUnescapestring());
            }


            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.OrderNumber))
            {
                var orderNumber = purchaseOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.SalesOrder.OrderNumber, $"%{orderNumber}%"));
            }

            var salesOrderItems = new SalesOrderItemList();
            return await salesOrderItems
                .Create(collectionBeforePaging, purchaseOrderResource.Skip, purchaseOrderResource.PageSize);
        }
    }
}
