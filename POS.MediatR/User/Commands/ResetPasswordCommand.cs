using POS.Data.Dto;
using MediatR;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class ResetPasswordCommand : IRequest<ServiceResponse<UserDto>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
