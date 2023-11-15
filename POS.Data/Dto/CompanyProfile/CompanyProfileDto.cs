using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class CompanyProfileDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CurrencyCode { get; set; }
    }
}
