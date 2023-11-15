using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrder.Commands
{
    public class GetSalesOrderItemsCommand : IRequest<List<SalesOrderItemDto>>
    {
        public Guid Id { get; set; }
        public bool IsReturn { get; set; }
    }
}
