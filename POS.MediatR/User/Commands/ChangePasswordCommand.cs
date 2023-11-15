using POS.Data.Dto;
using MediatR;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class ChangePasswordCommand : IRequest<ServiceResponse<UserDto>>
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
