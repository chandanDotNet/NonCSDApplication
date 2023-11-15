using System.Threading.Tasks;
using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Resources;

namespace POS.Repository
{
    public interface ILoginAuditRepository : IGenericRepository<LoginAudit>
    {
        Task<LoginAuditList> GetDocumentAuditTrails(LoginAuditResource loginAuditResrouce);
        Task LoginAudit(LoginAuditDto loginAudit);
    }
}
