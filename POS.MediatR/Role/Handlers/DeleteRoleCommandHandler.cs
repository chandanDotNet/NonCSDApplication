using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using POS.Repository;
using System;
using POS.Domain;
using POS.Common.UnitOfWork;
using POS.Helper;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Handlers
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ServiceResponse<RoleDto>>
    {
        private readonly UserInfoToken _userInfoToken;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeleteRoleCommandHandler> _logger;
        public DeleteRoleCommandHandler(
            UserInfoToken userInfoToken,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteRoleCommandHandler> logger
            )
        {
            _userInfoToken = userInfoToken;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<RoleDto>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _roleRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Not found");
                return ServiceResponse<RoleDto>.Return404();
            }

            if (entityExist.IsSuperRole)
            {
                _logger.LogError("Super admin Role can not be Deleted.");
                return ServiceResponse<RoleDto>.Return409("Super admin Role can not be Deleted.");
            }

            var exitingRole = await _userRoleRepository.All.AnyAsync(c => c.RoleId == entityExist.Id);
            if (exitingRole)
            {
                _logger.LogError("Role can not be Deleted because it is assign to User");
                return ServiceResponse<RoleDto>.Return409("Role can not be Deleted because it is assign to User");
            }
            entityExist.IsDeleted = true;
            entityExist.DeletedBy = Guid.Parse(_userInfoToken.Id);
            entityExist.DeletedDate = DateTime.UtcNow;
            _roleRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<RoleDto>.Return500();
            }
            return ServiceResponse<RoleDto>.ReturnResultWith204();
        }
    }
}
