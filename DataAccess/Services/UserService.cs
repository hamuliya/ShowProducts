using DataAccess.DbAccess;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly ISqlDataAccess _db;

        public UserService(ISqlDataAccess db)
        {
            _db = db;
        }


        public async Task<UserEntity> GetUserByNameAsync(string userName)
        {
            IEnumerable<UserEntity>? results = await _db.LoadDataAsync<UserEntity, dynamic>
           ("dbo.spUser_GetByName", new { UserName = userName });


            if (results is null) return null;
            return results.FirstOrDefault();

        }


        public async Task<int> InsertUserAsync(UserEntity user)
        {


            int userId = await _db.SaveDataAsync("dbo.spUser_Insert", new { user.UserId, user.UserName, user.PasswordHash, user.Salt, user.EmailAddress, user.FirstName, user.LastName });
            return userId;

        }




    }
}
