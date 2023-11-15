using System.Threading.Tasks;
using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Resources;

namespace POS.Repository
{
    public interface INLogRepository : IGenericRepository<NLog>
    {
        Task<NLogList> GetNLogsAsync(NLogResource nLogResource);
    }
}
