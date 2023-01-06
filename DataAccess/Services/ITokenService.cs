using DataAccess.Models;

namespace DataAccess.Data
{
    public interface ITokenService
    {
        Task<TokenEntity> GetRefreshTokenByUserIdAsync(int userId);
        Task InsertRefreshTokenAsync(TokenEntity token);
        Task UpdateRefreshTokenByUserIdAsync(TokenEntity token);
    }
}