using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.City.Commands
{
    public class AddCityCommand : IRequest<ServiceResponse<CityDto>>
    {
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
    }
}
