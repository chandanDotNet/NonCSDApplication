using MediatR;
using POS.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Inquiry.Commands
{
    public class GetInquiryProductsCommand : IRequest<List<ProductDto>>
    {
        public Guid Id { get; set; }
    }
}
