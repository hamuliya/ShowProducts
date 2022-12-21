using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

public class RefreshTokenData : IRefreshTokenData
{
    private readonly ISqlDataAccess _db;

    public RefreshTokenData(ISqlDataAccess db)
    {
        _db = db;
    }


    public async Task InsertRefreshToken(RefreshTokenDBModel refreshToken)
    {
        await _db.ExecData("dbo.spRefreshToken_Insert", new { refreshToken.UserId, refreshToken.RefreshToken, refreshToken.Expiry });
    }


    public async Task UpdateRefreshTokenByUserId(RefreshTokenDBModel refreshToken)
    {
        await _db.ExecData("dbo.spRefreshToken_UpdateByUserId", new { refreshToken.UserId, refreshToken.RefreshToken, refreshToken.Expiry });
    }

    public async Task<RefreshTokenDBModel> GetRefreshTokenByUserId(int userId)
    {
        IEnumerable<RefreshTokenDBModel> results = await _db.LoadData<RefreshTokenDBModel, dynamic>("dbo.spRefreshToken_GetByUserId", new { UserId = userId });
        return results.FirstOrDefault();
    }






}
