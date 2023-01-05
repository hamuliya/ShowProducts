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


    public async Task InsertTokenAsync(TokenEntity token)
    {
        await _db.ExecDataAsync("dbo.spToken_Insert", new { token.UserId, token.RefreshToken, token.RefreshTokenExpiry, token.AccessToken, token.AccessTokenExpiry });
    }


    public async Task UpdateTokenByUserIdAsync(TokenEntity token)
    {
        await _db.ExecDataAsync("dbo.spToken_UpdateByUserId", new { token.UserId, token.RefreshToken, token.RefreshTokenExpiry, token.AccessToken, token.AccessTokenExpiry });
    }

    public async Task<TokenEntity> GetTokenByUserIdAsync(int userId)
    {
        IEnumerable<TokenEntity> result = await _db.LoadDataAsync<TokenEntity, dynamic>("dbo.Token_GetByUserId", new { UserId = userId });
        if (result is null) return null;
        return result.FirstOrDefault();
    }






}
