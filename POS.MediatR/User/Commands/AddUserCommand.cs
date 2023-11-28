using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;
using POS.Helper;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class AddUserCommand : IRequest<ServiceResponse<UserDto>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public bool IsImageUpdate { get; set; }
        public string ImgSrc { get; set; }
        public Guid CounterId { get; set; }
        public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();

    }
}
