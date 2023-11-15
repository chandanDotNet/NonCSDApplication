using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetRoleUsersHandler : IRequestHandler<GetRoleUsersQuery, List<UserRoleDto>>
    {
        IUserRoleRepository _userRoleRepository;
        public GetRoleUsersHandler(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public Task<List<UserRoleDto>> Handle(GetRoleUsersQuery request, CancellationToken cancellationToken)
        {
            var userRoles = _userRoleRepository
                .AllIncluding(c => c.User)
                .Where(c => c.RoleId == request.RoleId)
                .Select(cs => new UserRoleDto
                {
                    UserId = cs.UserId,
                    RoleId = cs.RoleId,
                    UserName = cs.User.UserName,
                    FirstName = cs.User.FirstName,
                    LastName = cs.User.LastName
                }).ToList();

            return Task.FromResult(userRoles);

        }
    }
}
