using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class EmailOrPhoneExistCheckQueryHandler : IRequestHandler<EmailOrPhoneExistCheckQuery, bool>
    {
        private readonly ICustomerRepository _customerRepository;

        public EmailOrPhoneExistCheckQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public Task<bool> Handle(EmailOrPhoneExistCheckQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var result = _customerRepository.All.Where(c => c.Email == request.Email && c.Id != request.Id).Any();
                return Task.FromResult(result);
            }
            else
            {
                var result = _customerRepository.All.Where(c => c.MobileNo == request.MobileNo && c.Id != request.Id).Any();
                return Task.FromResult(result);
            }
        }
    }
}
