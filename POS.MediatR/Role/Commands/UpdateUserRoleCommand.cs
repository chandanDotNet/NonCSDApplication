using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateUserRoleCommand : IRequest<ServiceResponse<UserRoleDto>>
    {
        public Guid Id { get; set; }
        public List<UserRoleDto> UserRoles { get; set; }
    }
}
