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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace POS.MediatR.Brand.Handler
{
    public class GetAllBrandCommandHandler : IRequestHandler<GetAllBrandCommand, List<BrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllBrandCommandHandler(
           IBrandRepository brandRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<BrandDto>> Handle(GetAllBrandCommand request, CancellationToken cancellationToken)
        {
            var entities = await _brandRepository.All
                .Select(c => new BrandDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.BrandImagePath, c.ImageUrl) : ""
                }).ToListAsync();
            return entities;
        }
    }
}
