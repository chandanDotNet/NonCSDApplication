using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class SalesOrderPaymentDto
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Note { get; set; }
        public string AttachmentUrl { get; set; }
    }
}
