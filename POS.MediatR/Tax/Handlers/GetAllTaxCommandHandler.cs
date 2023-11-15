using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Tax.Commands;
using POS.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Tax.Handlers
{
    public class GetAllTaxCommandHandler : IRequestHandler<GetAllTaxCommand, List<TaxDto>>
    {
        private readonly ITaxRepository _taxRepository;
        private readonly IMapper _mapper;
        public GetAllTaxCommandHandler(
           ITaxRepository taxRepository,
            IMapper mapper)
        {
            _taxRepository = taxRepository;
            _mapper = mapper;
        }

        public async Task<List<TaxDto>> Handle(GetAllTaxCommand request, CancellationToken cancellationToken)
        {
            var entities = await _taxRepository.All.ProjectTo<TaxDto>(_mapper.ConfigurationProvider).ToListAsync();
            return entities;
        }
    }
}
