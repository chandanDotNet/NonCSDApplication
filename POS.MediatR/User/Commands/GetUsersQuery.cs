using MediatR;
using POS.Data.Resources;
using POS.Repository;

namespace POS.MediatR.CommandAndQuery
{
    public class GetUsersQuery : IRequest<UserList>
    {
        public UserResource UserResource { get; set; }
    }
}
