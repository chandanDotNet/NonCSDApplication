using POS.Data.Dto;
using MediatR;
using System;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class DeleteUserCommand : IRequest<ServiceResponse<UserDto>>
    {
        public Guid Id { get; set; }
    }
}
