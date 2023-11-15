using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Tax.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Tax.Handlers
{
    public class GetTaxCommandHandler : IRequestHandler<GetTaxCommand, ServiceResponse<TaxDto>>
    {
        private readonly ITaxRepository _taxRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTaxCommandHandler> _logger;
        public GetTaxCommandHandler(
           ITaxRepository taxRepository,
            IMapper mapper,
            ILogger<GetTaxCommandHandler> logger
            )
        {
            _taxRepository = taxRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<TaxDto>> Handle(GetTaxCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _taxRepository.FindBy(c => c.Id == request.Id)
                .ProjectTo<TaxDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Tax is not available");
                return ServiceResponse<TaxDto>.Return404();
            }
            return ServiceResponse<TaxDto>.ReturnResultWith200(entityDto);
        }
    }
}
