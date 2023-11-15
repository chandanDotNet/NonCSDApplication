using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class UpdateInquiryCommandHandler : IRequestHandler<UpdateInquiryCommand, ServiceResponse<InquiryDto>>
    {
        private readonly IInquiryRepository _inquiryRepository;
        private readonly IInquiryProductRepository _inquiryProductRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateInquiryCommandHandler> _logger;

        public UpdateInquiryCommandHandler(
            IInquiryRepository inquiryRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            IInquiryProductRepository inquiryProductRepository,
            ILogger<UpdateInquiryCommandHandler> logger)
        {
            _inquiryRepository = inquiryRepository;
            _uow = uow;
            _mapper = mapper;
            _inquiryProductRepository = inquiryProductRepository;
            _logger = logger;
        }

        public async Task<ServiceResponse<InquiryDto>> Handle(UpdateInquiryCommand request, CancellationToken cancellationToken)
        {
            request.InquiryProducts = request.InquiryProducts.DistinctBy(c => c.ProductId).ToList();
            var entityExist = await _inquiryRepository.AllIncluding(c => c.InquiryProducts).Where(c => c.Id == request.Id).FirstOrDefaultAsync();
            if (entityExist == null)
            {
                _logger.LogError("Inquiry does not exists.");
                return ServiceResponse<InquiryDto>.Return409("Inquiry does not exists.");
            }

            if (entityExist.InquiryProducts != null )
            {
                foreach (var inquiryProduct in entityExist.InquiryProducts)
                {
                    //if (!request.InquiryProducts.Any(se => se.ProductId == inquiryProduct.ProductId && se.InquiryId == inquiryProduct.InquiryId))
                    //{
                        _uow.Context.InquiryProducts.Remove(inquiryProduct);
                    //} 
                  
                }
            }
            if ( request.InquiryProducts != null)
            {
                foreach (var inquiryProduct in request.InquiryProducts)
                {
                  
                    _uow.Context.InquiryProducts.Add(new Data.InquiryProduct
                    {
                        ProductId = inquiryProduct.ProductId,
                        InquiryId= request.Id
                    });

                }
            }
                _mapper.Map(request, entityExist);
            _inquiryRepository.Update(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<InquiryDto>.Return500();
            }
            var industrydto = _mapper.Map<InquiryDto>(entityExist);
            return ServiceResponse<InquiryDto>.ReturnResultWith200(industrydto);
        }
    }
}
