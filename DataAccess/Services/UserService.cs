using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
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
           ("dbo.spUser_GetByName", new { userName = userName });


            if (results is null) return null;
            return results.FirstOrDefault();

        }


        public async Task<int> InsertUserAsync(UserEntity user)
        {


            int userId = await _db.SaveDataAsync("dbo.spUser_Insert", new { user.userId, user.userName, user.passwordHash, user.salt, user.emailAddress, user.firstName, user.lastName });
            return userId;

        }




    }
}
