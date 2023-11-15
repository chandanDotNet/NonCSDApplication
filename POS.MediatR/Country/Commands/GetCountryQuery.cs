using POS.Data.Dto;
using MediatR;
using System.Collections.Generic;
using System;

namespace POS.MediatR.CommandAndQuery
{
    public class GetCountryQuery : IRequest<List<CountryDto>>
    {
        public Guid Id { get; set; }
    }
}
