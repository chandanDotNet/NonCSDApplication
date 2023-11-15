using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class InventoryHistory : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public InventorySourceEnum InventorySource { get; set; }
        public decimal Stock { get; set; }
        public decimal PricePerUnit { get; set; }
        public Product Product { get; set; }
        public decimal PreviousTotalStock { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public Guid? SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
    }
}
