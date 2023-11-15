using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class InquiryResource : ResourceParameter
    {
        public InquiryResource() : base("CreatedDate")
        {
        }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string ContactPerson { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? AssignTo { get; set; }
        public Guid? InquiryStatusId { get; set; }
        public Guid? InquirySourceId { get; set; }
    }
}
