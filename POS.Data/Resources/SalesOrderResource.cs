using System;

namespace POS.Data.Resources
{
    public class SalesOrderResource : ResourceParameter
    {
        public SalesOrderResource() : base("SOCreatedDate")
        {
        }

        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime? SOCreatedDate { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsSalesOrderRequest { get; set; } = false;
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid? ProductId { get; set; }
        public SalesOrderStatus Status { get; set; } = SalesOrderStatus.All;
        public string ProductName { get; set; }
    }
}

