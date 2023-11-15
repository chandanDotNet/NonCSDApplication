using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface IPurchaseOrderPaymentRepository : IGenericRepository<PurchaseOrderPayment>
    {
        Task<PurchaseOrderPaymentList> GetAllPurchaseOrderPayments(PurchaseOrderResource purchaseOrderResource);
    }
}
