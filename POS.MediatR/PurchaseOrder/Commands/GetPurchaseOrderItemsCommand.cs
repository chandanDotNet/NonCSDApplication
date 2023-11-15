using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.PurchaseOrder.Commands
{
    public class GetPurchaseOrderItemsCommand : IRequest<List<PurchaseOrderItemDto>>
    {
        public Guid Id { get; set; }
        public bool IsReturn { get; set; }
    }
}
