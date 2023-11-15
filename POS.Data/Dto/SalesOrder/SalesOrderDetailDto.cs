using System;


namespace POS.Data.Dto
{
    public class SalesOrderDetailDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IsClosed { get; set; }
        public Guid CustomerId { get; set; }
        public int TotalQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public int InStockQuantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Tax { get; set; }
        public bool IsStockAtCustomerWarehouse { get; set; }
        public string CustomerInvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
