using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class InquirySource:BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
