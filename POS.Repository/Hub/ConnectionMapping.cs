using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using POS.Data.Dto;

namespace POS.Repository
{
    public class ConnectionMappingRepository : IConnectionMappingRepository
    {
        private bool _schedulerStatus = false;
        private bool _emailSchedulerStatus = false;

        private ConcurrentDictionary<string, SignlarUser> _onlineUser { get; set; } = new ConcurrentDictionary<string, SignlarUser>();
        public bool AddUpdate(SignlarUser tempUserInfo, string connectionId)
        {
            var userAlreadyExists = _onlineUser.ContainsKey(tempUserInfo.Id);

            var userInfo = new SignlarUser
            {
                Id = tempUserInfo.Id,
                ConnectionId = connectionId,
                Email = tempUserInfo.Email
            };

            _onlineUser.AddOrUpdate(tempUserInfo.Id, userInfo, (key, value) => userInfo);

            return userAlreadyExists;
        }
        public void Remove(SignlarUser tempUserInfo)
        {
            SignlarUser userInfo;
            _onlineUser.TryRemove(tempUserInfo.Id, out userInfo);
        }
        public IEnumerable<SignlarUser> GetAllUsersExceptThis(SignlarUser tempUserInfo)
        {
            return _onlineUser.Values.Where(item => item.Id != tempUserInfo.Id);
        }
        public SignlarUser GetUserInfo(SignlarUser tempUserInfo)
        {
            SignlarUser user;
            _onlineUser.TryGetValue(tempUserInfo.Id, out user);
            return user;
        }

        public SignlarUser GetUserInfoById(Guid userId)
        {
            SignlarUser user;
            _onlineUser.TryGetValue(userId.ToString(), out user);
            return user;
        }
        public SignlarUser GetUserInfoByName(string id)
        {
            SignlarUser user;
            _onlineUser.TryGetValue(id, out user);
            return user;
        }
        public SignlarUser GetUserInfoByConnectionId(string connectionId)
        {
            foreach (var onlineUser in _onlineUser)
            {
                var user = onlineUser.Value;
                if (user.ConnectionId == connectionId)
                {
                    return user;
                }
            }
            return null;
        }

        public void SetSchedulerServiceStatus(bool status)
        {
            _schedulerStatus = status;
        }

        public bool GetSchedulerServiceStatus()
        {
            return _schedulerStatus;
        }

        public void SetEmailSchedulerStatus(bool status)
        {
            _emailSchedulerStatus = status;
        }

        public bool GetEmailSchedulerStatus()
        {
            return _emailSchedulerStatus;
        }
    }
}
