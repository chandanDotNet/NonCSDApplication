using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class SalesOrderItemTax
    {
        public Guid Id { get; set; }
        public Guid SalesOrderItemId { get; set; }
        [ForeignKey("SalesOrderItemId")]
        public SalesOrderItem SalesOrderItem { get; set; }
        public Guid TaxId { get; set; }
        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }
    }
}
