using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IUserService
    {
        Task<UserEntity> GetUserByNameAsync(string userName);
        Task<int> InsertUserAsync(UserEntity user);
    }
}