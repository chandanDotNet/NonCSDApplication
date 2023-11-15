using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class SalesOrderRecentShipmentDate
    {
        public Guid SalesOrderId { get; set; }
        public string SalesOrderNumber { get; set; }
        public DateTime ExpectedShipmentDate { get; set; }
        public int Quantity { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
