using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.Extensions.Logging;

namespace POS.MediatR.Handlers
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResponse<RoleDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleClaimRepository _roleClaimRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly UserInfoToken _userInfoToken;
        private readonly ILogger<UpdateRoleCommandHandler> _logger;
        public UpdateRoleCommandHandler(
           IRoleRepository roleRepository,
           IRoleClaimRepository roleClaimRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            UserInfoToken userInfoToken,
            ILogger<UpdateRoleCommandHandler> logger
            )
        {
            _roleRepository = roleRepository;
            _roleClaimRepository = roleClaimRepository;
            _mapper = mapper;
            _uow = uow;
            _userInfoToken = userInfoToken;
            _logger = logger;
        }

        public async Task<ServiceResponse<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _roleRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                 .FirstOrDefaultAsync();

            if (entityExist != null)
            {
                _logger.LogError("Role Name Already Exist.");
                return ServiceResponse<RoleDto>.Return409("Role Name Already Exist.");
            }

            // Update Role
            entityExist = await _roleRepository.FindByInclude(v => v.Id == request.Id, c => c.RoleClaims).FirstOrDefaultAsync();

            if (entityExist.IsSuperRole)
            {
                _logger.LogError("Super admin Role can not be updated.");
                return ServiceResponse<RoleDto>.Return409("Super admin Role can not be updated.");
            }

            entityExist.Name = request.Name;
            entityExist.ModifiedBy = Guid.Parse(_userInfoToken.Id);
            entityExist.ModifiedDate = DateTime.UtcNow;
            entityExist.NormalizedName = request.Name;
            _roleRepository.Update(entityExist);

            // update Role Claim
            var roleClaims = entityExist.RoleClaims.ToList();
            var roleClaimsToAdd = request.RoleClaims.Where(c => !roleClaims.Select(c => c.Id).Contains(c.Id)).ToList();
            _roleClaimRepository.AddRange(_mapper.Map<List<RoleClaim>>(roleClaimsToAdd));
            var roleClaimsToDelete = roleClaims.Where(c => !request.RoleClaims.Select(cs => cs.Id).Contains(c.Id)).ToList();
            _roleClaimRepository.RemoveRange(roleClaimsToDelete);

            // TODO: update user Role
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<RoleDto>.Return500();
            }
            return ServiceResponse<RoleDto>.ReturnResultWith200(_mapper.Map<RoleDto>(entityExist));
        }
    }
}
