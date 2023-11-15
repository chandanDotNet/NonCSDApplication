using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class SearchCustomerQueryHandler : IRequestHandler<SearchCustomerQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        public SearchCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<List<CustomerDto>> Handle(SearchCustomerQuery request, CancellationToken cancellationToken)
        {
            var customers = _customerRepository.All;
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                //  request.SearchQuery = request.SearchQuery.Trim();
                customers = customers.Where(c => EF.Functions.Like(c.CustomerName, $"{request.SearchQuery}%"));
            }
            else if (request.SearchQuery.ToLower() == "customername")
            {
                customers = customers.Where(c => EF.Functions.Like(c.CustomerName, $"{request.SearchQuery}%"));
            }

            return await customers
                .OrderBy(c => c.CustomerName)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName
                }).Take(request.PageSize)
                    .ToListAsync();
        }
    }
}
