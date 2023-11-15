using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class GetSupplierQuery : IRequest<ServiceResponse<SupplierDto>>
    {
        public Guid Id { get; set; }
    }
}
