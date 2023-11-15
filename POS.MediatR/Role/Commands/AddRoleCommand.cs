using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class AddRoleCommand : IRequest<ServiceResponse<RoleDto>>
    {
        public string Name { get; set; }
        public List<RoleClaimDto> RoleClaims { get; set; } = new List<RoleClaimDto>();
    }
}
