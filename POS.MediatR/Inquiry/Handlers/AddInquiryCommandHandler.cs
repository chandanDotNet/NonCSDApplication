using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddInquiryCommandHandler : IRequestHandler<AddInquiryCommand, ServiceResponse<InquiryDto>>
    {

        private readonly IInquiryRepository _inquiryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;

        public AddInquiryCommandHandler(
            IInquiryRepository inquiryRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper)
        {
            _inquiryRepository = inquiryRepository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<InquiryDto>> Handle(AddInquiryCommand request, CancellationToken cancellationToken)
        {
            request.InquiryProducts = request.InquiryProducts.DistinctBy(c => c.ProductId).ToList();
            var entity = _mapper.Map<Data.Inquiry>(request);
            _inquiryRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<InquiryDto>.Return500();
            }
            var industrydto = _mapper.Map<InquiryDto>(entity);
            return ServiceResponse<InquiryDto>.ReturnResultWith200(industrydto);
        }


    }
}
