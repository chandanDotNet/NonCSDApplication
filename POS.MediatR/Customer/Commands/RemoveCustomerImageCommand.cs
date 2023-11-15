using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class RemoveCustomerImageCommand : IRequest<ServiceResponse<string>>
    {
        public Guid Id { get; set; }
    }
}
