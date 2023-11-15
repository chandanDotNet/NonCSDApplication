using POS.Data;
using POS.Repository;
using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllCustomerQuery : IRequest<CustomerList>
    {
        public CustomerResource CustomerResource { get; set; }
    }
}
