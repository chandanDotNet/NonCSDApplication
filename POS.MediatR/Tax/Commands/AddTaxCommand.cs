using MediatR;
using POS.Data.Dto;
using POS.Helper;


namespace POS.MediatR.Tax.Commands
{
    public class AddTaxCommand : IRequest<ServiceResponse<TaxDto>>
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }
    }
}
