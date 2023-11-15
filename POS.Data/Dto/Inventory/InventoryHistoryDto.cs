using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class InventoryHistoryDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public InventorySourceEnum InventorySource { get; set; }
        public decimal Stock { get; set; }
        public decimal PricePerUnit { get; set; }
        public long PreviousTotalStock { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public string SalesOrderNumber { get; set; }
        public Guid? SalesOrderId { get; set; }
    }
}
