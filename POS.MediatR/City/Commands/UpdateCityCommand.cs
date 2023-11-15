using MediatR;
using POS.Helper;
using System;

namespace POS.MediatR.City.Commands
{
    public class UpdateCityCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
    }
}
