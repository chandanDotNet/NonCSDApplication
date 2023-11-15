using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Helper;
using MediatR;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllContactUsQuery : IRequest<PagedList<ContactRequest>>
    {
        public ContactUsResource ContactUsResource { get; set; }
    }
}
