using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.Tax.Commands
{
   public class GetTaxCommand: IRequest<ServiceResponse<TaxDto>>
    {
        public Guid Id { get; set; }
    }
}
