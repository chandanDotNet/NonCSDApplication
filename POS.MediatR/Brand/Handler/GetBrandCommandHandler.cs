using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.Brand.Command;
using POS.MediatR.Tax.Commands;
using POS.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Brand.Handler
{
    public class GetBrandCommandHandler 
        : IRequestHandler<GetBrandCommand, ServiceResponse<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBrandCommandHandler> _logger;
        public GetBrandCommandHandler(
           IBrandRepository brandRepository,
            IMapper mapper,
            ILogger<GetBrandCommandHandler> logger
            )
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<BrandDto>> Handle(GetBrandCommand request, CancellationToken cancellationToken)
        {
            var entityDto = await _brandRepository.FindBy(c => c.Id == request.Id)
                .ProjectTo<BrandDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            if (entityDto == null)
            {
                _logger.LogError("Brand is not exists");
                return ServiceResponse<BrandDto>.Return404();
            }
            return ServiceResponse<BrandDto>.ReturnResultWith200(entityDto);
        }
    }
}
