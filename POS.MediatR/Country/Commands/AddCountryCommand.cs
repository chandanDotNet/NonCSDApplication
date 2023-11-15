using MediatR;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.Country.Commands
{
    public class AddCountryCommand : IRequest<ServiceResponse<CountryDto>>
    {
        public string CountryName { get; set; }
    }
}
