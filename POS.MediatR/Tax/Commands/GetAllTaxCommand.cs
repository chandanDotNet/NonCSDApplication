using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System.Collections.Generic;


namespace POS.MediatR.Tax.Commands
{
    public class GetAllTaxCommand : IRequest<List<TaxDto>>
    {
    }
}

