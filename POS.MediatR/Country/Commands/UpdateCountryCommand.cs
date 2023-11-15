using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.Country.Commands
{
    public class UpdateCountryCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
    }
}
