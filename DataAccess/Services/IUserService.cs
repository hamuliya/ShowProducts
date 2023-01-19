using DataAccess.Entities;

namespace DataAccess.Services
{
    public interface IUserService
    {
        Task<UserEntity> GetUserByNameAsync(string userName);
        Task<int> InsertUserAsync(UserEntity user);
    }
}