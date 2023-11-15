using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class InquiryProduct
    {
        public Guid ProductId { get; set; }
        public Guid InquiryId { get; set; }
        public Product Product { get; set; }
        public Inquiry Inquiry { get; set; }
    }
}
