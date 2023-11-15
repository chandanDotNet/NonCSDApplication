using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class PurchaseOrderRecentDeliverySchedule
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime ExpectedDispatchDate { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
