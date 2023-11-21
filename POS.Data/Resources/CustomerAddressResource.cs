using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class CustomerAddressResource : ResourceParameters
    {
        public CustomerAddressResource() : base("CustomerId")
        {
        }
        public string CustomerName { get; set; }
        public Guid Id { get; set; }
        public string HouseNo { get; set; }
        public string StreetDetails { get; set; }
        public string LandMark { get; set; }
        public string Type { get; set; }
        public Guid? CustomerId { get; set; }
        public bool IsPrimary { get; set; }
        
    }
}
