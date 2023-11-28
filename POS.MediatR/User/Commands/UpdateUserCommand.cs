using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateUserCommand : IRequest<ServiceResponse<UserDto>>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public bool IsImageUpdate { get; set; }
        public string ImgSrc { get; set; }
        public Guid? CounterId { get; set; }
        public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
    }
}
