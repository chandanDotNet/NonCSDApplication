using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class PurchaseOrderPaymentRepository
        : GenericRepository<PurchaseOrderPayment, POSDbContext>, IPurchaseOrderPaymentRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;
        public PurchaseOrderPaymentRepository(IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService)
          : base(uow)
        {
            _propertyMappingService = propertyMappingService;
        }

        public async Task<PurchaseOrderPaymentList> GetAllPurchaseOrderPayments(PurchaseOrderResource purchaseOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.PurchaseOrder).ApplySort(purchaseOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<PurchaseOrderPaymentDto, PurchaseOrderPayment>());
          

            if (purchaseOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PaymentDate >= new DateTime(purchaseOrderResource.FromDate.Value.Year, purchaseOrderResource.FromDate.Value.Month, purchaseOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (purchaseOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PaymentDate <= new DateTime(purchaseOrderResource.ToDate.Value.Year, purchaseOrderResource.ToDate.Value.Month, purchaseOrderResource.ToDate.Value.Day, 23, 59, 59));
            }


            var purchaseOrderPaymentList = new PurchaseOrderPaymentList();
            return await purchaseOrderPaymentList
                .Create(collectionBeforePaging, purchaseOrderResource.Skip, purchaseOrderResource.PageSize);
        }
    }
}
