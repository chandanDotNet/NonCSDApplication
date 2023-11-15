using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetTestimonialsQueryHandler : IRequestHandler<GetTestimonialsQuery, ServiceResponse<TestimonialsDto>>
    {
        private readonly ITestimonialsRepository _testimonialsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTestimonialsQueryHandler> _logger;
        private readonly PathHelper _pathHelper;

        public GetTestimonialsQueryHandler(ITestimonialsRepository testimonialsRepository,
            IMapper mapper,
            ILogger<GetTestimonialsQueryHandler> logger,
            PathHelper pathHelper)
        {
            _testimonialsRepository = testimonialsRepository;
            _mapper = mapper;
            _logger = logger;
            _pathHelper = pathHelper;
        }
        public async Task<ServiceResponse<TestimonialsDto>> Handle(GetTestimonialsQuery request, CancellationToken cancellationToken)
        {
            var testimonial = await _testimonialsRepository.FindAsync(request.Id);
            if (testimonial == null)
            {
                _logger.LogError("Testimonial Not found", request);
                return ServiceResponse<TestimonialsDto>.Return404("Testimonial Not found");
            }
            if (!string.IsNullOrWhiteSpace(testimonial.Url))
            {
                testimonial.Url = Path.Combine(_pathHelper.TestimonialsImagePath, testimonial.Url);
            }
            return ServiceResponse<TestimonialsDto>.ReturnResultWith200(_mapper.Map<TestimonialsDto>(testimonial));
        }
    }
}
