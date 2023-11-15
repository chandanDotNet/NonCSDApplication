using MediatR;
using POS.Helper;
using System;
namespace POS.MediatR.Country.Commands
{
    public class DeleteCountryCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
