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
    public class AddLoginPageBannerCommandHandler : IRequestHandler<AddLoginPageBannerCommand, ServiceResponse<LoginPageBannerDto>>
    {
        private readonly ILoginPageBannerRepository _loginPageBannerRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddLoginPageBannerCommandHandler> _logger;
        private readonly PathHelper _pathHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddLoginPageBannerCommandHandler(
            ILoginPageBannerRepository loginPageBannerRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddLoginPageBannerCommandHandler> logger,
            PathHelper pathHelper,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _loginPageBannerRepository = loginPageBannerRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
            _pathHelper = pathHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ServiceResponse<LoginPageBannerDto>> Handle(AddLoginPageBannerCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _loginPageBannerRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Login Banner Already Exist");
                return ServiceResponse<LoginPageBannerDto>.Return409("Login Banner Already Exist.");
            }
            var entity = _mapper.Map<Data.LoginPageBanner>(request);

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entity.ImageUrl = Guid.NewGuid().ToString() + ".png";
            }

            _loginPageBannerRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Save Page have Error");
                return ServiceResponse<LoginPageBannerDto>.Return500();
            }

            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, _pathHelper.LoginPageBannerImagePath);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                await FileData.SaveFile(Path.Combine(pathToSave, entity.ImageUrl), request.ImageUrlData);
            }
            var entityToReturn = _mapper.Map<LoginPageBannerDto>(entity);
            if (!string.IsNullOrWhiteSpace(request.ImageUrlData))
            {
                entityToReturn.ImageUrl = Path.Combine(_pathHelper.LoginPageBannerImagePath, entityToReturn.ImageUrl);
            }
            return ServiceResponse<LoginPageBannerDto>.ReturnResultWith200(entityToReturn);
        }
    }
}