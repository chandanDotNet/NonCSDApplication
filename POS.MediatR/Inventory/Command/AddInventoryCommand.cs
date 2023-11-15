using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Inventory.Command
{
    public class AddInventoryCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid ProductId { get; set; }
        public long Stock { get; set; }
        public decimal PricePerUnit { get; set; }
        public Guid UnitId { get; set; }
    }
}
