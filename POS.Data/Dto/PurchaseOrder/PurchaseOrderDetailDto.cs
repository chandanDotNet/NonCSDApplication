using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
   public class PurchaseOrderDetailDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime POCreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IsClosed { get; set; }
        public Guid SupplierId { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int InStockQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Tax { get; set; }
        public bool IsStockAtSupplierWarehouse { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public string SupplierName { get; set; }
        public string PackagingTypeName { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid PackagingTypeId { get; set; }
        public SupplierDto Supplier { get; set; }
    }
}
