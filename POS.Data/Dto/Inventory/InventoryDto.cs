using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class InventoryDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public decimal Stock { get; set; }
        public decimal PricePerUnit { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public decimal AverageSalesPrice { get; set; }
        public InventorySourceEnum InventorySource { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public Guid? SalesOrderId { get; set; }
        public Guid UnitId { get; set; }
        public Guid? WarehouseId { get; set; }
        public decimal TaxValue { get; set; }
        public decimal Discount { get; set; }
    }
}
