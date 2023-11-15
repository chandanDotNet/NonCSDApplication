using POS.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateInquirySourceCommand : IRequest<ServiceResponse<bool>>
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
