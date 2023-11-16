using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface ICustomerAddressRepository : IGenericRepository<CustomerAddress>
    {
        Task<CustomerAddressList> GetCustomerAddresses(CustomerAddressResource customerAddressResource);
    }
}
