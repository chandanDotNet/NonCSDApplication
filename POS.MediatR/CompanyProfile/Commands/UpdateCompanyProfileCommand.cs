using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateCompanyProfileCommand : IRequest<ServiceResponse<CompanyProfileDto>>
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public string ImageData { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CurrencyCode { get; set; }
    }
}
