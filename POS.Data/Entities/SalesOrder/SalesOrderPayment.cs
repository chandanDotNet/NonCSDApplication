using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class SalesOrderPayment : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Note { get; set; }
        public string AttachmentUrl { get; set; }
    }
}
