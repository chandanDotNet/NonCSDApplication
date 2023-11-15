using POS.Data.Entities;
using System;
using System.Collections.Generic;

namespace POS.Data.Dto
{
    public class SalesOrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public PurchaseSaleItemStatusEnum Status { get; set; }
        public CustomerDto Customer { get; set; }
        public Guid SalesOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TaxValue { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public Guid UnitId { get; set; }
        public string SalesOrderNumber { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public decimal Total { get; set; }
        public string CustomerName { get; set; }
        public Guid? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public UnitConversationDto UnitConversation { get; set; }
        public List<SalesOrderItemTaxDto> SalesOrderItemTaxes { get; set; }
    }
}
