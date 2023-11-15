using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Brand.Command;
using POS.MediatR.Country.Command;
using POS.MediatR.Tax.Commands;
using POS.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace POS.MediatR.Country.Handler
{
    public class GetAllCountryCommandHandler : IRequestHandler<GetAllCountryCommand, List<CountryDto>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllCountryCommandHandler(
           ICountryRepository countryRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<CountryDto>> Handle(GetAllCountryCommand request, CancellationToken cancellationToken)
        {
            var entities = await _countryRepository.All
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    CountryName = c.CountryName,
                   
                }).ToListAsync();
            return entities;
        }
    }
}
