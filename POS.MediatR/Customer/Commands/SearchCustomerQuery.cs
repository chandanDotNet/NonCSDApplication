using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;


namespace POS.MediatR.CommandAndQuery
{
    public class SearchCustomerQuery : IRequest<List<CustomerDto>>
    {
        public string SearchQuery { get; set; }
        public string SearchBy { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
