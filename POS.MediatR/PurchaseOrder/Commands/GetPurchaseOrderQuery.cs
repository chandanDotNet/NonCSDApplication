using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class GetPurchaseOrderQuery : IRequest<ServiceResponse<PurchaseOrderDto>>
    {
        public Guid Id { get; set; }
    }
}
