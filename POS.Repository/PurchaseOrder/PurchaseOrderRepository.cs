using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class PurchaseOrderRepository
        : GenericRepository<PurchaseOrder, POSDbContext>, IPurchaseOrderRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public PurchaseOrderRepository(IUnitOfWork<POSDbContext> uow,
            IPropertyMappingService propertyMappingService) : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<PurchaseOrderList> GetAllPurchaseOrders(PurchaseOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Supplier).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<PurchaseOrderDto, PurchaseOrder>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsPurchaseOrderRequest == purchaseOrderResource.IsPurchaseOrderRequest);

            if (purchaseOrderResource.Status != PurchaseOrderStatus.All)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Status == purchaseOrderResource.Status);
            }

            if (purchaseOrderResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SupplierId == purchaseOrderResource.SupplierId);
            }

            if (purchaseOrderResource.ProductId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PurchaseOrderItems.Any(c => c.ProductId == purchaseOrderResource.ProductId));
            }

            if (purchaseOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate >= new DateTime(purchaseOrderResource.FromDate.Value.Year, purchaseOrderResource.FromDate.Value.Month, purchaseOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (purchaseOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate <= new DateTime(purchaseOrderResource.ToDate.Value.Year, purchaseOrderResource.ToDate.Value.Month, purchaseOrderResource.ToDate.Value.Day, 23, 59, 59));
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.SupplierName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Supplier.SupplierName == purchaseOrderResource.SupplierName.GetUnescapestring());
            }

            if (purchaseOrderResource.POCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate == purchaseOrderResource.POCreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.OrderNumber))
            {
                var orderNumber = purchaseOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderNumber, $"%{orderNumber}%"));
            }


            var purchaseOrders = new PurchaseOrderList();
            return await purchaseOrders
                .Create(collectionBeforePaging, purchaseOrderResource.Skip, purchaseOrderResource.PageSize);
        }



        public async Task<PurchaseOrderList> GetAllPurchaseOrdersReport(PurchaseOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.Supplier, c => c.PurchaseOrderItems).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<PurchaseOrderDto, PurchaseOrder>());


            collectionBeforePaging = collectionBeforePaging
               .Where(a => a.IsPurchaseOrderRequest == purchaseOrderResource.IsPurchaseOrderRequest);

            if (purchaseOrderResource.SupplierId.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.SupplierId == purchaseOrderResource.SupplierId);
            }



            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.SupplierName))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Supplier.SupplierName == purchaseOrderResource.SupplierName.GetUnescapestring());
            }

            if (purchaseOrderResource.POCreatedDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.POCreatedDate == purchaseOrderResource.POCreatedDate);
            }

            if (!string.IsNullOrWhiteSpace(purchaseOrderResource.OrderNumber))
            {
                var orderNumber = purchaseOrderResource.OrderNumber.Trim();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => EF.Functions.Like(a.OrderNumber, $"%{orderNumber}%"));
            }


            var purchaseOrders = new PurchaseOrderList();
            return await purchaseOrders
                .Create(collectionBeforePaging, 0, 0);
        }
    }
}
