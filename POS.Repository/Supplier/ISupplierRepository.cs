using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        Task<SupplierList> GetSuppliers(SupplierResource supplierResource);
        Task<SupplierPaymentList> GetSuppliersPayment(SupplierResource supplierResource);
    }
}
