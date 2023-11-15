using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddInquiryAttachmentCommandHandler : IRequestHandler<AddInquiryAttachmentCommand, ServiceResponse<InquiryAttachmentDto>>
    {
        private readonly IInquiryAttachmentRepository _inquiryAttachmentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PathHelper _pathHelper;
        private readonly ILogger<AddInquiryAttachmentCommandHandler> _logger;

        public AddInquiryAttachmentCommandHandler(
            IInquiryAttachmentRepository inquiryAttachmentRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper,
            ILogger<AddInquiryAttachmentCommandHandler> logger)
        {
            _inquiryAttachmentRepository = inquiryAttachmentRepository;
            _uow = uow;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
            _logger = logger;
        }

        public async Task<ServiceResponse<InquiryAttachmentDto>> Handle(AddInquiryAttachmentCommand request, CancellationToken cancellationToken)
        {
            string contentRootPath = _webHostEnvironment.WebRootPath;
            var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);
            var extension = request.Extension;
            var id = Guid.NewGuid();
            var path = $"{id}.{extension}";
            var documentPath = Path.Combine(pathToSave, path);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            string base64 = request.Documents.Split(',').LastOrDefault();
            if (!string.IsNullOrWhiteSpace(base64))
            {
                byte[] bytes = Convert.FromBase64String(base64);
                await System.IO.File.WriteAllBytesAsync($"{documentPath}", bytes);
            }
            var inquiryAttachment = new InquiryAttachment
            {
                Id = id,
                Name = request.Name,
                InquiryId = request.InquiryId,
                Path = path,
            };
            _inquiryAttachmentRepository.Add(inquiryAttachment);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Creating a Inquiry Attachment failed on save.", request);
                return ServiceResponse<InquiryAttachmentDto>.ReturnFailed(500, $"Creating a Inquiry Attachment  failed on save.");
            }

            return ServiceResponse<InquiryAttachmentDto>.ReturnResultWith200(_mapper.Map<InquiryAttachmentDto>(inquiryAttachment));
        }
    }
}
