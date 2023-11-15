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
    public class SalesOrderPaymentRepository : GenericRepository<SalesOrderPayment, POSDbContext>, ISalesOrderPaymentRepository
    {
        private readonly IPropertyMappingService _propertyMappingService;

        public SalesOrderPaymentRepository(IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService)
          : base(uow)
        {
            _propertyMappingService= propertyMappingService;
        }


        public async Task<SaleOrderPaymentList> GetAllSaleOrderPayments(SalesOrderResource salesOrderResource)
        {
            var collectionBeforePaging = AllIncluding(c => c.SalesOrder).ApplySort(salesOrderResource.OrderBy,
                _propertyMappingService.GetPropertyMapping<SalesOrderPaymentDto, SalesOrderPayment>());


            if (salesOrderResource.FromDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PaymentDate >= new DateTime(salesOrderResource.FromDate.Value.Year, salesOrderResource.FromDate.Value.Month, salesOrderResource.FromDate.Value.Day, 0, 0, 1));
            }
            if (salesOrderResource.ToDate.HasValue)
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.PaymentDate <= new DateTime(salesOrderResource.ToDate.Value.Year, salesOrderResource.ToDate.Value.Month, salesOrderResource.ToDate.Value.Day, 23, 59, 59));
            }


            var saleOrderPaymentList = new SaleOrderPaymentList();
            return await saleOrderPaymentList
                .Create(collectionBeforePaging, salesOrderResource.Skip, salesOrderResource.PageSize);
        }
    }
}
