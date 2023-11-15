using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POS.Data.Dto;

namespace POS.Repository
{
    public class UserHub : Hub<IHubClient>
    {
        private IConnectionMappingRepository _userInfoInMemory;

        public UserHub(IConnectionMappingRepository userInfoInMemory)
        {
            _userInfoInMemory = userInfoInMemory;
        }

        public async Task Leave(string id)
        {
            var userInfo = _userInfoInMemory.GetUserInfoByName(id);
            _userInfoInMemory.Remove(userInfo);
            await Clients.AllExcept(new List<string> { Context.ConnectionId })
                .UserLeft(id);
        }
        public async Task Logout(string id)
        {
            var userInfo = _userInfoInMemory.GetUserInfoByName(id);
            if (userInfo != null)
            {
                _userInfoInMemory.Remove(userInfo);
                await Clients.AllExcept(new List<string> { Context.ConnectionId })
                    .UserLeft(id);
            }
        }
        public async Task ForceLogout(string id)
        {
            var userInfo = _userInfoInMemory.GetUserInfoByName(id);
            if (userInfo != null)
            {
                _userInfoInMemory.Remove(userInfo);

                await Clients.Client(userInfo.ConnectionId)
                       .ForceLogout(userInfo);

                await Clients.AllExcept(new List<string> { userInfo.ConnectionId })
                    .UserLeft(id);
            }
        }

        public async Task Join(SignlarUser userInfo)
        {
            if (!_userInfoInMemory.AddUpdate(userInfo, Context.ConnectionId))
            {
                // new user
                await Clients.AllExcept(new List<string> { Context.ConnectionId })
                    .NewOnlineUser(_userInfoInMemory.GetUserInfo(userInfo));
            }
            else
            {
                // existing user joined again
            }

            await Clients.Client(Context.ConnectionId)
                .Joined(_userInfoInMemory.GetUserInfo(userInfo));

            await Clients.Client(Context.ConnectionId)
                .OnlineUsers(_userInfoInMemory.GetAllUsersExceptThis(userInfo));
        }

        public Task SendDirectMessage(string message, string targetUserName)
        {
            var userInfoSender = _userInfoInMemory.GetUserInfoByConnectionId(Context.ConnectionId);
            var userInfoReciever = _userInfoInMemory.GetUserInfoByName(targetUserName);
            return Clients.Client(userInfoReciever.ConnectionId).SendDM(message, userInfoSender);
        }

        public async Task SendNotification(Guid userId)
        {
            var userInfoReciever = _userInfoInMemory.GetUserInfoById(userId);
            await Clients.Client(userInfoReciever.ConnectionId).SendNotification(userId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userInfo = _userInfoInMemory.GetUserInfoByConnectionId(Context.ConnectionId);
            if (userInfo == null)
                return;
            _userInfoInMemory.Remove(userInfo);
            await Clients.AllExcept(new List<string> { userInfo.ConnectionId }).UserLeft(userInfo.Id);
        }
    }
}
