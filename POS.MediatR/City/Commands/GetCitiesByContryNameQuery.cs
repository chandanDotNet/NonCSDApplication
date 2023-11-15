using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetCitiesByContryNameQuery : IRequest<ServiceResponse<List<CityDto>>>
    {
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
}
