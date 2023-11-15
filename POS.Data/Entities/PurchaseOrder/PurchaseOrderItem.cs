using POS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class PurchaseOrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public PurchaseSaleItemStatusEnum Status { get; set; } = PurchaseSaleItemStatusEnum.Not_Return;
        public Guid PurchaseOrderId { get; set; }
        [ForeignKey("PurchaseOrderId")]
        public PurchaseOrder PurchaseOrder { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public List<PurchaseOrderItemTax> PurchaseOrderItemTaxes { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid UnitId { get; set; }
        [ForeignKey("UnitId")]
        public UnitConversation UnitConversation { get; set; }
        public Guid? WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public Warehouse Warehouse { get; set; } = null;

    }
}
