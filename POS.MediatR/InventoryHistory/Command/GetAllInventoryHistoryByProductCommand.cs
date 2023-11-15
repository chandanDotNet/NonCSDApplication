using MediatR;
using POS.Data;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.InventoryHistory.Command
{
    public class GetAllInventoryHistoryByProductCommand : IRequest<InventoryHistoryList>
    {
        public InventoryHistoryResource InventoryHistoryResource { get; set; }
    }
}
