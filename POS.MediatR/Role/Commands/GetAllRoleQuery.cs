using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllRoleQuery : IRequest<List<RoleDto>>
    {
    }
}
