using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using POS.Helper;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface IInquiryRepository : IGenericRepository<Inquiry>
    {
        Task<InquiryList> GetInquiries(InquiryResource inquiryResource);
    }
}
