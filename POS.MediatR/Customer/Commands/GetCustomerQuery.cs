using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class GetCustomerQuery : IRequest<ServiceResponse<CustomerDto>>
    {
        public Guid Id { get; set; }
    }
}
