using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IUserData
    {
        Task<UserDBModel> GetUserByName(string userName);
        int InsertUser(UserDBModel user);
    }
}