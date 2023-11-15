using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class Inventory : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal Stock { get; set; }
        public Product Product { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public decimal AverageSalesPrice { get; set; }
    }
}
