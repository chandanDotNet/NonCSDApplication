using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Resources;
using POS.Repository;
using POS.API.Helpers;

namespace POS.API.Controllers
{
    /// <summary>
    /// User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        public IMediator _mediator { get; set; }
        public readonly UserInfoToken _userInfo;
        /// <summary>
        /// User
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userInfo"></param>
        public UserController(
            IMediator mediator,
            UserInfoToken userInfo
            )
        {
            _mediator = mediator;
            _userInfo = userInfo;
        }
        /// <summary>
        ///  Create a User
        /// </summary>
        /// <param name="addUserCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [ClaimCheck("USR_ADD_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> AddUser(AddUserCommand addUserCommand)
        {
            var result = await _mediator.Send(addUserCommand);
            if (!result.Success)
            {
                return ReturnFormattedResponse(result);
            }
            return CreatedAtAction("GetUser", new { id = result.Data.Id }, result.Data);
        }


        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetAllUsers()
        {
            var getAllUserQuery = new GetAllUserQuery { };
            var result = await _mediator.Send(getAllUserQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUser")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var getUserQuery = new GetUserQuery { Id = id };
            var result = await _mediator.Send(getUserQuery);
            return ReturnFormattedResponse(result);
        }
        
        /// <summary>
        /// Get Use Notification Coung.
        /// </summary>
        /// <returns></returns>
        [HttpGet("notification/count")]
        [Produces("application/json", "application/xml", Type = typeof(int))]
        public async Task<IActionResult> GetUserNotificationCount()
        {
            var getUserNotificationCountQuery = new GetUserNotificationCountQuery {  };
            var result = await _mediator.Send(getUserNotificationCountQuery);
            return Ok(result);
        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <param name="userResource"></param>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(UserList))]
        public async Task<IActionResult> GetUsers([FromQuery] UserResource userResource)
        {
            var getAllLoginAuditQuery = new GetUsersQuery
            {
                UserResource = userResource
            };
            var result = await _mediator.Send(getAllLoginAuditQuery);

            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                skip = result.Skip,
                totalPages = result.TotalPages
            };
            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        /// <summary>
        /// Get Recently Registered Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRecentlyRegisteredUsers")]
        [ClaimCheck("USR_VIEW_USERS")]
        [Produces("application/json", "application/xml", Type = typeof(List<UserDto>))]
        public async Task<IActionResult> GetRecentlyRegisteredUsers()
        {
            var getRecentlyRegisteredUserQuery = new GetRecentlyRegisteredUserQuery { };
            var result = await _mediator.Send(getRecentlyRegisteredUserQuery);
            return Ok(result);
        }


        /// <summary>
        /// Update User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserCommand"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ClaimCheck("USR_UPDATE_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand updateUserCommand)
        {
            updateUserCommand.Id = id;
            var result = await _mediator.Send(updateUserCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUserProfileCommand"></param>
        /// <returns></returns>
        [HttpPut("profile")]
        [ClaimCheck("USR_UPDATE_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileCommand updateUserProfileCommand)
        {
            var result = await _mediator.Send(updateUserProfileCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Update Profile photo
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateUserProfilePhoto"), DisableRequestSizeLimit]
        [ClaimCheck("USR_UPDATE_USER")]
        [Produces("application/json", "application/xml", Type = typeof(UserDto))]
        public async Task<IActionResult> UpdateUserProfilePhoto()
        {
            var updateUserProfilePhotoCommand = new UpdateUserProfilePhotoCommand()
            {
                FormFile = Request.Form.Files,
            };
            var result = await _mediator.Send(updateUserProfilePhotoCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Delete User By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [ClaimCheck("USR_DELETE_USER")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var deleteUserCommand = new DeleteUserCommand { Id = Id };
            var result = await _mediator.Send(deleteUserCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// User Change Password
        /// </summary>
        /// <param name="resetPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost("changepassword")]
        [ClaimCheck("USR_RESET_PWD")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand resetPasswordCommand)
        {
            var result = await _mediator.Send(resetPasswordCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Reset Resetpassword
        /// </summary>
        /// <param name="newPasswordCommand"></param>
        /// <returns></returns>
        [HttpPost("resetpassword")]
        [ClaimCheck("USR_RESET_PWD")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand newPasswordCommand)
        {
            var result = await _mediator.Send(newPasswordCommand);
            return ReturnFormattedResponse(result);
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet("profile")]
        [ClaimCheck("USR_VIEW_USERS")]
        public async Task<IActionResult> GetProfile()
        {
            var getUserQuery = new GetUserQuery
            {
                Id = Guid.Parse(_userInfo.Id)
            };
            var result = await _mediator.Send(getUserQuery);
            return ReturnFormattedResponse(result);
        }

    }
}
