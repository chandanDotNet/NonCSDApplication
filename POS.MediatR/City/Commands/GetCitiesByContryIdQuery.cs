using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetCitiesByContryIdQuery : IRequest<List<CityDto>>
    {
        public Guid CountryId { get; set; }
        public string CityName { get; set; }
    }
}
