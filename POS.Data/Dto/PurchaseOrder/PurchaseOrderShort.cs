using System;

namespace POS.Data.Dto
{
    public class PurchaseOrderShort
    {
        public Guid PurchaseOrderId { get; set; }
        public string PurchaseOrderName { get; set; }
        public int Quantity { get; set; }
        public string SupplierName { get; set; }
        public Guid SupplierId { get; set; }
    }
}
