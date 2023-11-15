using POS.Data;
using POS.Data.Resources;
using POS.Helper;
using POS.Repository;
using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllInquiryQuery : IRequest<InquiryList>
    {
        public InquiryResource InquiryResource { get; set; }
    }
}
