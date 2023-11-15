using Microsoft.EntityFrameworkCore;
using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public  class SalesOrderRepository 
        : GenericRepository<SalesOrder, POSDbContext>, ISalesOrderRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public SalesOrderRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }
        public async Task<SalesOrderList> GetAllSalesOrders(SalesOrderResource salesOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Customer).ApplySort(salesOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<SalesOrderDto, SalesOrder>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsSalesOrderRequest == salesOrderResource.IsSalesOrderRequest);

            if (salesOrderResource.Status != SalesOrderStatus.All)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == salesOrderResource.Status);
            }

            if (salesOrderResource.CustomerId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.CustomerId == salesOrderResource.CustomerId);
            }

            if (salesOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SalesOrderItems.Any(c => c.ProductId == salesOrderResource.ProductId));
            }

            if (salesOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate >= new DateTime(salesOrderResource.FromDate.Value.Year, salesOrderResource.FromDate.Value.Month, salesOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (salesOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate <= new DateTime(salesOrderResource.ToDate.Value.Year, salesOrderResource.ToDate.Value.Month, salesOrderResource.ToDate.Value.Day, 23, 59, 59));
            }


            if (!string.IsNullOrWhiteSpace(salesOrderResource.CustomerName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Customer.CustomerName == salesOrderResource.CustomerName.GetUnescapestring());
            }

            if (salesOrderResource.SOCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SOCreatedDate == salesOrderResource.SOCreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(salesOrderResource.OrderNumber))
            {
                var orderNumber = salesOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderNumber, $"%{orderNumber}%"));
            }


            var salesOrders = new SalesOrderList();
            return await salesOrders
                .Create(collectionBeforePaging, salesOrderResource.Skip, salesOrderResource.PageSize);
        }
    }
}
