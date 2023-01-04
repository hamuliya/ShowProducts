using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ISqlDataAccess _db;

    public RefreshTokenService(ISqlDataAccess db)
    {
        _db = db;
    }


    public async Task InsertRefreshTokenAsync(RefreshTokenEntity refreshToken)
    {
        await _db.ExecDataAsync("dbo.spRefreshToken_Insert", new { refreshToken.UserId, refreshToken.RefreshToken, refreshToken.Expiry });
    }


    public async Task UpdateRefreshTokenByUserIdAsync(RefreshTokenEntity refreshToken)
    {
        await _db.ExecDataAsync("dbo.spRefreshToken_UpdateByUserId", new { refreshToken.UserId, refreshToken.RefreshToken, refreshToken.Expiry });
    }

    public async Task<RefreshTokenEntity> GetRefreshTokenByUserIdAsync(int userId)
    {
        IEnumerable<RefreshTokenEntity> result = await _db.LoadDataAsync<RefreshTokenEntity, dynamic>("dbo.spRefreshToken_GetByUserId", new { UserId = userId });
        if (result is null) return null;
        return result.FirstOrDefault();
    }






}
