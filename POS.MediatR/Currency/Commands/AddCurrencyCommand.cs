using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Currency.Commands
{
    public class AddCurrencyCommand: IRequest<ServiceResponse<CurrencyDto>>
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
