using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }


        public async Task<UserDBModel> GetUserByName(string userName)
        {
            IEnumerable<UserDBModel>? results = await _db.LoadData<UserDBModel, dynamic>
           ("dbo.spUser_GetByName", new { UserName = userName });
            return results.FirstOrDefault();

        }


        public int InsertUser(UserDBModel user)
        {


            int userId = _db.SaveData("dbo.spUser_Insert", new { user.UserId, user.UserName, user.PasswordHash, user.Salt, user.EmailAddress, user.FirstName, user.LastName });
            return userId;

        }




    }
}
