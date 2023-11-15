using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.Currency.Commands
{
    public class GetCurrencyCommand: IRequest<ServiceResponse<CurrencyDto>>
    {
        public Guid Id { get; set; }
    }
}
