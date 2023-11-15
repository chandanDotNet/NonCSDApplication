using System;


namespace POS.Data.Dto
{
    public class SalesOrderItemTaxDto
    {
        public Guid Id { get; set; }
        public Guid SalesOrderItemId { get; set; }
        public Guid TaxId { get; set; }
        public string TaxName { get; set; }
        public decimal TaxPercentage { get; set; }
        public TaxDto Tax { get; set; }
    }
}
