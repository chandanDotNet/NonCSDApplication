using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateRoleCommand: IRequest<ServiceResponse<RoleDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RoleClaimDto> RoleClaims { get; set; }
    }
}
