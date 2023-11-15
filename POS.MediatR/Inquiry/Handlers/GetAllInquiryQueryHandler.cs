using AutoMapper;
using POS.Data;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllInquiryQueryHandler : IRequestHandler<GetAllInquiryQuery, InquiryList>
    {

        private readonly IInquiryRepository _inquiryRepository;
        private readonly IMapper _mapper;

        public GetAllInquiryQueryHandler(
            IInquiryRepository inquiryRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService)
        {
            _inquiryRepository = inquiryRepository;
            _mapper = mapper;
        }

        public async Task<InquiryList> Handle(GetAllInquiryQuery request, CancellationToken cancellationToken)
        {
            return await _inquiryRepository.GetInquiries(request.InquiryResource);
        }
    }
}
