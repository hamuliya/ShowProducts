using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IRefreshTokenService
    {
        Task<TokenEntity> GetRefreshTokenByUserIdAsync(int userId);
        Task InsertRefreshTokenAsync(TokenEntity refreshToken);
        Task UpdateRefreshTokenByUserIdAsync(TokenEntity refreshToken);
    }
}