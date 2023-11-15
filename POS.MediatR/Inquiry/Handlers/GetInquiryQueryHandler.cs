using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.Repository;

namespace POS.MediatR.Handlers
{
    public class GetInquiryQueryHandler : IRequestHandler<GetInquiryQuery, ServiceResponse<InquiryDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetInquiryQueryHandler> _logger;
        private readonly IInquiryRepository _inquiryRepository;

        public GetInquiryQueryHandler(IInquiryRepository inquiryRepository,
            IMapper mapper,
            ILogger<GetInquiryQueryHandler> logger)
        {
            _inquiryRepository = inquiryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<InquiryDto>> Handle(GetInquiryQuery request, CancellationToken cancellationToken)
        {
            var inquiryEntity = await _inquiryRepository
                .All
                .Include(c => c.InquiryProducts)
                .ThenInclude(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (inquiryEntity == null)
            {
                _logger.LogError("Inquiry not found");
                return ServiceResponse<InquiryDto>.Return404();
            }
            var inquiryDto = _mapper.Map<InquiryDto>(inquiryEntity);
            inquiryDto.InquiryProducts = inquiryEntity.InquiryProducts.Select(c => new InquiryProductDto
            {
                ProductId = c.ProductId,
                Name = c.Product.Name,
                InquiryId = c.InquiryId
            }).ToList();
            return ServiceResponse<InquiryDto>.ReturnResultWith200(inquiryDto);
        }
    }
}
