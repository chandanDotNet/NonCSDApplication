using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace POS.Data
{
    public class PurchaseOrderItemTax 
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderItemId { get; set; }
        [ForeignKey("PurchaseOrderItemId")]
        public PurchaseOrderItem PurchaseOrderItem { get; set; }
        public Guid TaxId { get; set; }
        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }
    }
}
