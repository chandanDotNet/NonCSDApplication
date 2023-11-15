using POS.Helper;
using System;

namespace POS.Data.Resources
{
    public class SupplierResource : ResourceParameters
    {
        public SupplierResource() : base("SupplierName")
        {

        }
        public Guid? Id { get; set; }
        public string SupplierName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Country { get; set; }
    }
}
