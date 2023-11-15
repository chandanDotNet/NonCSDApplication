using POS.Common.GenericRepository;
using POS.Data;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<CustomerList> GetCustomers(CustomerResource customerResource);
        Task<CustomerPaymentList> GetCustomersPayment(CustomerResource customerResource);
        Task<CustomerPaymentList> GetCustomersPaymentReport(CustomerResource customerResource);
        Task<CustomerList> GetCustomersData(CustomerResource customerResource);
    }
}
