using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface ISalesOrderItemRepository : IGenericRepository<SalesOrderItem>
    {
        Task<SalesOrderItemList> GetAllSalesOrderItems(SalesOrderResource purchaseOrderResource);
    }
}
