using System;

namespace POS.Data.Dto.SalesOrder
{
    public class SalesOrderShort
    {
        public Guid SalesOrderId { get; set; }
        public string SalesOrderName { get; set; }
        public int Quantity { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerId { get; set; }
    }
}
