using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using Microsoft.AspNetCore.Authorization;
using POS.API.Helpers;

namespace POS.API.Controllers.Email
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailSMTPSettingController : BaseController
    {
        IMediator _mediator;
        public EmailSMTPSettingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create an Email SMTP Configuration.
        /// </summary>
        /// <param name="addEmailSMTPSettingCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
        public async Task<IActionResult> AddEmailSMTPSetting(AddEmailSMTPSettingCommand addEmailSMTPSettingCommand)
        {
            var result = await _mediator.Send(addEmailSMTPSettingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Email SMTP Configuration.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
        public async Task<IActionResult> GetEmailSMTPSetting(Guid id)
        {
            var query = new GetEmailSMTPSettingQuery() { Id = id };
            var result = await _mediator.Send(query);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get Email SMTP Configuration list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(List<EmailSMTPSettingDto>))]
        public async Task<IActionResult> GetEmailSMTPSettings()
        {
            var query = new GetEmailSMTPSettingsQuery() { };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Update an Email SMTP Configuration.
        /// </summary>
        /// <param name="addEmailSMTPSettingCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
        public async Task<IActionResult> UpdateEmailSMTPSetting(Guid id, UpdateEmailSMTPSettingCommand updateEmailSMTPSettingCommand)
        {
            updateEmailSMTPSettingCommand.Id = id;
            var result = await _mediator.Send(updateEmailSMTPSettingCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete an Email SMTP Configuration.
        /// </summary>
        /// <param name="addEmailSMTPSettingCommand"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ClaimCheck("EMAIL_MANAGE_EMAIL_SMTP_SETTINS")]
        [Produces("application/json", "application/xml", Type = typeof(EmailSMTPSettingDto))]
        public async Task<IActionResult> DeleteEmailSMTPSetting(Guid id)
        {
            var deleteEmailSMTPSettingCommand = new DeleteEmailSMTPSettingCommand() { Id = id };
            var result = await _mediator.Send(deleteEmailSMTPSettingCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
