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
        await _db.execDataAsync("dbo.spRefreshToken_Insert", new { refreshToken.userId, refreshToken.refreshToken, refreshToken.expiry });
    }


    public async Task UpdateRefreshTokenByUserIdAsync(RefreshTokenEntity refreshToken)
    {
        await _db.execDataAsync("dbo.spRefreshToken_UpdateByUserId", new { refreshToken.userId, refreshToken.refreshToken, refreshToken.expiry });
    }

    public async Task<RefreshTokenEntity> GetRefreshTokenByUserIdAsync(int userId)
    {
        IEnumerable<RefreshTokenEntity> result = await _db.loadDataAsync<RefreshTokenEntity, dynamic>("dbo.spRefreshToken_GetByUserId", new { userId = userId });
        if (result is null) return null;
        return result.FirstOrDefault();
    }






}
