using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class InventoryResource : ResourceParameters
    {
        public InventoryResource() : base("ProductName")
        {

        }
        public string ProductName { get; set; }
    }
}
