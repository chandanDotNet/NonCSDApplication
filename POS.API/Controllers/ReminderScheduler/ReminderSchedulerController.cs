using POS.Data.Dto;
using POS.Data.Entities;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReminderSchedulerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReminderSchedulerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add Reminder Scheduler.
        /// </summary>
        /// <param name="addReminderSchedulerCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(bool))]
        public async Task<IActionResult> CreateReminderScheduler(AddReminderSchedulerCommand addReminderSchedulerCommand)
        {
            var result = await _mediator.Send(addReminderSchedulerCommand);
            return Ok(result);
        }


        /// <summary>
        /// Get Reminder Schedulers.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="referenceId"></param>
        /// <returns></returns>
        [HttpGet("{application}/{referenceId}")]
        [Produces("application/json", "application/xml", Type = typeof(List<ReminderSchedulerDto>))]
        public async Task<IActionResult> GetReminderScheduler(ApplicationEnums application, Guid referenceId)
        {
            var getAllReminderSchedulerQuery = new GetAllReminderSchedulerQuery
            {
                Application = application,
                ReferenceId = referenceId
            };

            var result = await _mediator.Send(getAllReminderSchedulerQuery);
            return Ok(result);
        }
    }
}
