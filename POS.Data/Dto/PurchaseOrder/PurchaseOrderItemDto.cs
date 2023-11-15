using POS.Data.Entities;
using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class PurchaseOrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public int Quantity { get; set; }
        public decimal TaxValue { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public Guid UnitId { get; set; }
        public decimal Total { get; set; } = 0;
        public DateTime POCreatedDate { get; set; }
        public string SupplierName { get; set; }
        public PurchaseSaleItemStatusEnum Status { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public UnitConversationDto UnitConversation { get; set; }
        public List<PurchaseOrderItemTaxDto> PurchaseOrderItemTaxes { get; set; }
    }
}
