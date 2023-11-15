using POS.Data.Dto;
using MediatR;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateUserProfileCommand : IRequest<ServiceResponse<UserDto>>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
