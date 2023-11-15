using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class InventoryHistoryResource : ResourceParameters
    {
        public InventoryHistoryResource() : base("CreatedDate")
        {
        }
        public Guid ProductId { get; set; }
    }
}
