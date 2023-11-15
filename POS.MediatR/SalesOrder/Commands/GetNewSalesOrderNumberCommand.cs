using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class GetNewSalesOrderNumberCommand : IRequest<string>
    {
    }
}
