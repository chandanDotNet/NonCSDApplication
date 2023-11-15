using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.Warehouse.Commands
{
    public class GetWarehouseCommand : IRequest<ServiceResponse<WarehouseDto>>
    {
        public Guid Id { get; set; }
    }
}
