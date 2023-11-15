using MediatR;
using POS.Helper;
using System;

namespace POS.MediatR.Currency.Commands
{
    public class DeleteCurrencyCommand: IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
