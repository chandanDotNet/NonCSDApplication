using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Dto;
using POS.MediatR.UnitConversation.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS.API.Controllers.UnitConversation
{
    /// <summary>
    /// Category Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitConversationController : BaseController
    {
        private readonly IMediator _mediator;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public UnitConversationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all Unit Conversation
        /// </summary>
        /// <param name="getAllUnitConversationCommand"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json", "application/xml", Type = typeof(List<UnitConversationDto>))]
        public async Task<IActionResult> GetUnitConversations([FromQuery] GetAllUnitConversationCommand getAllUnitConversationCommand)
        {
            var result = await _mediator.Send(getAllUnitConversationCommand);
            return Ok(result);
        }
   
        /// <summary>
        /// Create Unit Conversation
        /// </summary>
        /// <param name="addUnitConversationCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json", "application/xml", Type = typeof(UnitConversationDto))]
        public async Task<IActionResult> AddUnitConversation(AddUnitConversationCommand addUnitConversationCommand)
        {
            var response = await _mediator.Send(addUnitConversationCommand);
            if (!response.Success)
            {
                return ReturnFormattedResponse(response);
            }
            return ReturnFormattedResponse(response);
        }

        /// <summary>
        /// Update Unit Conversation
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="updateUnitConversationCommand"></param>
        /// <returns></returns>
        [HttpPut("{Id}")]
        [Produces("application/json", "application/xml", Type = typeof(UnitConversationDto))]
        public async Task<IActionResult> UpdateUnitConversation(Guid Id, UpdateUnitConversationCommand updateUnitConversationCommand)
        {
            updateUnitConversationCommand.Id = Id;
            var result = await _mediator.Send(updateUnitConversationCommand);
            return ReturnFormattedResponse(result);

        }

        /// <summary>
        ///  Delete Unit Conversation
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUnitConversation(Guid Id)
        {
            var deleteUnitConversationCommand = new DeleteUnitConversationCommand { Id = Id };
            var result = await _mediator.Send(deleteUnitConversationCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getAllBaseUnitConversationCommand"></param>
        /// <returns></returns>
        [HttpGet("dropDown")]
        [Produces("application/json", "application/xml", Type = typeof(List<UnitConversationDto>))]

        public async Task<IActionResult> GetUnitConversations([FromQuery] GetAllBaseUnitConversationCommand getAllBaseUnitConversationCommand)
        {
            var result = await _mediator.Send(getAllBaseUnitConversationCommand);
            return Ok(result);
        }
    }
}