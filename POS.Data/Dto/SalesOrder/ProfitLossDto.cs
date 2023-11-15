using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class ProfitLossDto
    {
        public decimal Total { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal PaidPayment { get; set; }
        public int TotalItem { get; set; }
    }
}
