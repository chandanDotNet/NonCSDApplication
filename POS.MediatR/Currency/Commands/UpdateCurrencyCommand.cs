using MediatR;
using POS.Helper;
using System;

namespace POS.MediatR.Currency.Commands
{
    public class UpdateCurrencyCommand: IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
