using MediatR;
using POS.Data.Resources;
using POS.Repository;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllLoginAuditQuery : IRequest<LoginAuditList>
    {
        public LoginAuditResource LoginAuditResource { get; set; }
    }
}
