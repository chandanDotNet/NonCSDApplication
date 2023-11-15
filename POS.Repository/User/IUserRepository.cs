using POS.Common.GenericRepository;
using POS.Data;
using POS.Data.Dto;
using System.Threading.Tasks;
using POS.Data.Resources;

namespace POS.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserList> GetUsers(UserResource userResource);
        Task<UserAuthDto> BuildUserAuthObject(User appUser);
    }
}
