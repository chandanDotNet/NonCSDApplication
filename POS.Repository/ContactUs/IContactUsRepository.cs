using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;
using POS.Helper;
using System.Threading.Tasks;

namespace POS.Repository
{
    public interface IContactUsRepository : IGenericRepository<ContactRequest>
    {
        Task<PagedList<ContactRequest>> GetContactUsList(ContactUsResource contactUsResource);
    }
}
