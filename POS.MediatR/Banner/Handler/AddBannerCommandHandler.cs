using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Banner.Command;
using POS.MediatR.Banner.Handler;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Banner.Handler
{
    public class AddBannerCommandHandler : IRequestHandler<AddBannerCommand, ServiceResponse<BannerDto>>
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBannerCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddBannerCommandHandler(
           IBannerRepository bannerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddBannerCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<BannerDto>> Handle(AddBannerCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _bannerRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Banner Already Exist");
                return ServiceResponse<BannerDto>.Return409("Banner Already Exist.");
            }
            var entity = _mapper.Map<Data.Banner>(request);

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            }

            _bannerRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Save Page have Error");
                return ServiceResponse<BannerDto>.Return500();
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.BannerImagePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            }
            var entityToReturn = _mapper.Map<BannerDto>(entity);
            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entityToReturn.ImageUrl = Path.Combine(_pathHelper.BannerImagePath, entityToReturn.ImageUrl);
            }
            return ServiceResponse<BannerDto>.ReturnResultWith200(entityToReturn);
        }
    }
}