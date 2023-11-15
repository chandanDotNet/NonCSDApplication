using POS.Data.Dto;
using POS.Data.Entities;
using MediatR;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllInquiryStatusQuery : IRequest<List<InquiryStatusDto>>
    {
    }
}
