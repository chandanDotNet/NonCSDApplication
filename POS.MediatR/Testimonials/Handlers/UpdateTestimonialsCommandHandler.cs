using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class UpdateTestimonialsCommandHandler : IRequestHandler<UpdateTestimonialsCommand, ServiceResponse<TestimonialsDto>>
    {
        private readonly ITestimonialsRepository _testimonialsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTestimonialsCommandHandler> _logger;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;

        public UpdateTestimonialsCommandHandler(ITestimonialsRepository testimonialsRepository,
            IMapper mapper,
            ILogger<UpdateTestimonialsCommandHandler> logger,
            IUnitOfWork<POSDbContext> uow,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _testimonialsRepository = testimonialsRepository;
            _mapper = mapper;
            _logger = logger;
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }
        public async Task<ServiceResponse<TestimonialsDto>> Handle(UpdateTestimonialsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _testimonialsRepository
               .FindByInclude(c => c.Id == request.Id)
               .FirstOrDefaultAsync();
            if (entity == null)
            {
                _logger.LogError("Testimonial Not found", request);
                return ServiceResponse<TestimonialsDto>.Return404("Testimonial Not found.");
            }

            var testimonial = _mapper.Map(request, entity);
            var imageName = Guid.NewGuid().ToString() + ".png";
            var oldImageName = testimonial.Url;
            if (request.IsImageUpload)
            {
                if (!string.IsNullOrWhiteSpace(request.ImageSrc))
                {
                    testimonial.Url = imageName;
                }
                else
                {
                    testimonial.Url = string.Empty;
                }
            }
            _testimonialsRepository.Update(testimonial);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while updating Testimonial", request);
                return ServiceResponse<TestimonialsDto>.ReturnFailed(500, $"Error while updating Testimonial.");
            }

            if (request.IsImageUpload)
            {
                string contentRootPath = _webHostEnvironment.WebRootPath;
                if (!string.IsNullOrWhiteSpace(request.ImageSrc))
                {
                    var pathToSave = Path.Combine(contentRootPath, _pathHelper.TestimonialsImagePath, testimonial.Url);
                    await FileData.SaveFile(pathToSave, request.ImageSrc);
                }

                var pathToDelete = Path.Combine(contentRootPath, _pathHelper.TestimonialsImagePath, oldImageName);
                if (File.Exists(pathToDelete))
                {
                    File.Delete(pathToDelete);
                }
            }
            return ServiceResponse<TestimonialsDto>.ReturnResultWith200(_mapper.Map<TestimonialsDto>(testimonial));
        }
    }
}
