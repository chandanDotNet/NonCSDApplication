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
    public class AddInquirySourceCommand : IRequest<ServiceResponse<InquirySourceDto>>
    {
        public string Name { get; set; }
    }
}
