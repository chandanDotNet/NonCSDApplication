using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;

namespace POS.MediatR.Warehouse.Commands
{
    public class GetWarehouseProductItemsCommand : IRequest<List<WarehouseInventoryDto>>
    {
        public Guid Id { get; set; }
    }
}
