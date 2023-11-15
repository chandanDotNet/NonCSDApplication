using MediatR;
using POS.Data;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Inventory.Command
{
    public class GetAllInventoryCommand : IRequest<InventoryList>
    {
        public InventoryResource InventoryResource { get; set; }
    }
}
