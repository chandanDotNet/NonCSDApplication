using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POS.Helper;

namespace POS.MediatR.Handlers
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, ServiceResponse<RoleDto>>
    {

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoleQueryHandler> _logger;

        public GetRoleQueryHandler(
           IRoleRepository roleRepository,
            IMapper mapper,
            ILogger<GetRoleQueryHandler> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<RoleDto>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var entity = await _roleRepository.AllIncluding(c => c.UserRoles, cs => cs.RoleClaims)
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync();
            if (entity != null)
                return ServiceResponse<RoleDto>.ReturnResultWith200(_mapper.Map<RoleDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<RoleDto>.Return404();
            }
        }
    }
}
