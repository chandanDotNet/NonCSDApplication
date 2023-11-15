using MediatR;
using POS.Data.Resources;
using POS.Repository;

namespace POS.MediatR.CommandAndQuery
{
    public class GetNLogsQuery : IRequest<NLogList>
    {
        public NLogResource NLogResource { get; set; }
    }
}
