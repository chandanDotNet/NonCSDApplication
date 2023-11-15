using POS.Helper;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using POS.API.Helpers;

namespace POS.API.Controllers.InquiryAttachment
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InquiryAttachmentController : BaseController
    {
        private IMediator _mediator;
        private readonly ILogger<InquiryAttachmentController> _logger;
        private IWebHostEnvironment _webHostEnvironment;
        private PathHelper _pathHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public InquiryAttachmentController(
            IMediator mediator,
            ILogger<InquiryAttachmentController> logger,
            IWebHostEnvironment webHostEnvironment,
            PathHelper pathHelper)
        {
            _mediator = mediator;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _pathHelper = pathHelper;
        }

        /// <summary>
        /// Add Inquiry Inquiry Attachment
        /// </summary>
        /// <param name="addInquiryAttachmentCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("INQ_ADD_INQUIRY")]
        public async Task<IActionResult> AddInquiryAttachment([FromBody] AddInquiryAttachmentCommand addInquiryAttachmentCommand)
        {
            var result = await _mediator.Send(addInquiryAttachmentCommand);
            if (result.StatusCode != 200)
            {
                _logger.LogError(result.StatusCode,
                                JsonSerializer.Serialize(result), "");
                return StatusCode(result.StatusCode, result);
            }
            return Ok(result.Data);
        }
        /// <summary>
        /// Get Inquiry Attachements By Id
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>

        [HttpGet("{inquiryId}")]
        [ClaimCheck("INQ_VIEW_INQUIRIES")]
        public async Task<IActionResult> GetInquiryAttachmentByInquiryId(Guid inquiryId)
        {
            var getInquiryAttachmentsByInquiryIdQuery = new GetInquiryAttachmentsByInquiryIdQuery
            {
                InquiryId = inquiryId
            };
            var result = await _mediator.Send(getInquiryAttachmentsByInquiryIdQuery);
            return Ok(result);
        }

        /// <summary>
        /// Deletes the inquiry.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("INQ_DELETE_INQUIRY")]
        public async Task<IActionResult> DeleteInquiryAttachment(Guid id)
        {
            var command = new DeleteInquiryAttachmentCommand() { Id = id };
            var result = await _mediator.Send(command);
            return ReturnFormattedResponse(result);
        }

        [HttpGet("{id}/download")]
        [ClaimCheck("INQ_VIEW_INQUIRIES")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var commnad = new GetInquiryAttachmentPathQuery
            {
                Id = id,
            };

            string contentRootPath = _webHostEnvironment.WebRootPath;
            var pathToSave = Path.Combine(contentRootPath, _pathHelper.Attachments);

            var path = await _mediator.Send(commnad);
            path = Path.Combine(pathToSave, path);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), path);

            if (!System.IO.File.Exists(filePath))
                return NotFound("File not found.");

            byte[] newBytes;
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] bytes = new byte[stream.Length];
                int numBytesToRead = (int)stream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                newBytes = bytes;
            }
            return File(newBytes, GetContentType(filePath), filePath);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

    }
}
