using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class SendEmail: BaseEntity
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Guid? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
        public Guid? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public bool IsSend { get; set; }
    }
}
