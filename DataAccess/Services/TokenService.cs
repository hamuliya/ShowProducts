using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

public class TokenService : ITokenService
{
    private readonly ISqlDataAccess _db;

    public TokenService(ISqlDataAccess db)
    {
        _db = db;
    }


    public async Task InsertRefreshTokenAsync(TokenEntity token)
    {
        await _db.ExecDataAsync("dbo.spRefreshToken_Insert", new { token.UserId, token.RefreshToken, token.RefreshTokenExpiry });
    }


    public async Task UpdateRefreshTokenByUserIdAsync(TokenEntity token)
    {
        await _db.ExecDataAsync("dbo.spRefreshToken_UpdateByUserId", new { token.UserId, token.RefreshToken, token.RefreshTokenExpiry });
    }

    public async Task<TokenEntity> GetRefreshTokenByUserIdAsync(int userId)
    {
        IEnumerable<TokenEntity> result = await _db.LoadDataAsync<TokenEntity, dynamic>("dbo.spRefreshToken_GetByUserId", new { UserId = userId });
        if (result is null) return null;
        return result.FirstOrDefault();
    }






}
