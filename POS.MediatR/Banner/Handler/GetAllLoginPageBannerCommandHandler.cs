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
    public class GetAllLoginPageBannerCommandHandler : IRequestHandler<GetAllLoginPageBannerCommand, List<LoginPageBannerDto>>
    {
        private readonly ILoginPageBannerRepository _loginPageBannerRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetAllLoginPageBannerCommandHandler(
           ILoginPageBannerRepository loginPageBannerRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _loginPageBannerRepository = loginPageBannerRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }

        public async Task<List<LoginPageBannerDto>> Handle(GetAllLoginPageBannerCommand request, CancellationToken cancellationToken)
        {
            var entities = await _loginPageBannerRepository.All
                .Select(c => new LoginPageBannerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ImageUrl) ? Path.Combine(_pathHelper.BannerImagePath, c.ImageUrl) : ""
                }).ToListAsync();
            return entities;
        }
    }
}
