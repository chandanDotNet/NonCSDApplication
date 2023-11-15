using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllInquirySourceQuery : IRequest<List<InquirySourceDto>>
    {
    }
}
