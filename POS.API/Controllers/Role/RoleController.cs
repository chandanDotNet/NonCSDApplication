using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.API.Helpers;

namespace POS.API.Controllers
{
    /// <summary>
    /// Role
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : BaseController
    {
        public IMediator _mediator { get; set; }
        /// <summary>
        /// Role
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public RoleController(
            IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Create A Role
        /// </summary>
        /// <param name="addRoleCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("ROLES_ADD_ROLE")]
        [Produces("application/json", "application/xml", Type = typeof(RoleDto))]
        public async Task<IActionResult> AddRole(AddRoleCommand addRoleCommand)
        {
            var result = await _mediator.Send(addRoleCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return CreatedAtAction("GetRole", new { id = result.Data.Id }, result.Data);
        }
        /// <summary>
        /// Update Exist Role By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateRoleCommand"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        [ClaimCheck("ROLES_UPDATE_ROLE")]
        [Produces("application/json", "application/xml", Type = typeof(RoleDto))]
        public async Task<IActionResult> UpdateRole(Guid id, UpdateRoleCommand updateRoleCommand)
        {
            updateRoleCommand.Id = id;
            var result = await _mediator.Send(updateRoleCommand);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Get Role By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}", Name = "GetRole")]
        [ClaimCheck("ROLES_VIEW_ROLES")]
        [Produces("application/json", "application/xml", Type = typeof(RoleDto))]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var getRoleQuery = new GetRoleQuery { Id = id };

            var result = await _mediator.Send(getRoleQuery);
            return ReturnFormattedResponse(result);
        }
        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetRoles")]
        [ClaimCheck("ROLES_VIEW_ROLES")]
        [Produces("application/json", "application/xml", Type = typeof(List<RoleDto>))]
        public async Task<IActionResult> GetRoles()
        {
            var getAllRoleQuery = new GetAllRoleQuery { };
            var result = await _mediator.Send(getAllRoleQuery);
            return Ok(result);
        }
        /// <summary>
        /// Delete Role By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ClaimCheck("ROLES_DELETE_ROLE")]
        public async Task<IActionResult> DeleteRole(Guid Id)
        {
            var deleteRoleCommand = new DeleteRoleCommand { Id = Id };
            var result = await _mediator.Send(deleteRoleCommand);
            return ReturnFormattedResponse(result);
        }
    }
}
