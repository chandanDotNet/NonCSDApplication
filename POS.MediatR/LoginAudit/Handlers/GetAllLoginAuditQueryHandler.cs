using MediatR;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.CommandAndQuery;
using POS.Repository;

namespace POS.MediatR.Handlers
{
    public class GetAllLoginAuditQueryHandler : IRequestHandler<GetAllLoginAuditQuery, LoginAuditList>
    {
        private readonly ILoginAuditRepository _loginAuditRepository;
        public GetAllLoginAuditQueryHandler(ILoginAuditRepository loginAuditRepository)
        {
            _loginAuditRepository = loginAuditRepository;
        }
        public async Task<LoginAuditList> Handle(GetAllLoginAuditQuery request, CancellationToken cancellationToken)
        {
            return await _loginAuditRepository.GetDocumentAuditTrails(request.LoginAuditResource);
        }
    }
}
