using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllTestimonialsQueryHandler : IRequestHandler<GetAllTestimonialsQuery, List<TestimonialsDto>>
    {
        private readonly ITestimonialsRepository _testimonialsRepository;
        private readonly PathHelper _pathHelper;
        private readonly IMapper _mapper;

        public GetAllTestimonialsQueryHandler(ITestimonialsRepository testimonialsRepository,
            PathHelper pathHelper,
            IMapper mapper)
        {
            _testimonialsRepository = testimonialsRepository;
            _pathHelper = pathHelper;
            _mapper = mapper;
        }

        public async Task<List<TestimonialsDto>> Handle(GetAllTestimonialsQuery request, CancellationToken cancellationToken)
        {
            var testimonials = await _testimonialsRepository.All.ToListAsync();
            testimonials.ForEach(testimonial =>
            {
                testimonial.Url = string.IsNullOrWhiteSpace(testimonial.Url) ? _pathHelper.NoImageFound
                : Path.Combine(_pathHelper.TestimonialsImagePath, testimonial.Url);
            });

            return _mapper.Map<List<TestimonialsDto>>(testimonials);
        }
    }
}
