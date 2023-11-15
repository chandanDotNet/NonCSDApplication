using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class GetSalesOrderCommand : IRequest<ServiceResponse<SalesOrderDto>>
    {
        public Guid Id { get; set; }
    }
}
