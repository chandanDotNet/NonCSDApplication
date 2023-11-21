using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class PaymentCardResource : ResourceParameters
    {
        public PaymentCardResource() : base("CustomerId")
        {
        }
        public string CustomerName { get; set; }
        public Guid Id { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CardType { get; set; }
        public string NickNameForCard { get; set; }
        public string CVV { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsPrimary { get; set; }
    }
}
