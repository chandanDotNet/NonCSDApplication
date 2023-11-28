using AutoMapper;
using MediatR;
using POS.Data.Dto;
using POS.MediatR.Banner.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POS.Helper;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.Banner.Handler
{
    public class GetAllBannerCommandHandler : IRequestHandler<GetAllBannerCommand, List<BannerDto>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllBannerCommandHandler(
           IBannerRepository bannerRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<BannerDto>> Handle(GetAllBannerCommand request, CancellationToken cancellationToken)
        {
            var entities = await _bannerRepository.All
                .Select(c => new BannerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.BannerImagePath, c.ImageUrl) : ""
                }).ToListAsync();
            return entities;
        }
    }
}
