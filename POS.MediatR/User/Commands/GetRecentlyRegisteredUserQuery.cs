using MediatR;
using System.Collections.Generic;
using POS.Data.Dto;

namespace POS.MediatR.CommandAndQuery
{
    public class GetRecentlyRegisteredUserQuery : IRequest<List<UserDto>>
    {
    }
}
