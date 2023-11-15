using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllActionQuery : IRequest<ServiceResponse<List<ActionDto>>>
    {
    }
}
