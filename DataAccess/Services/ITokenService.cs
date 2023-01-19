using DataAccess.Entities;

namespace DataAccess.Services
{
    public interface ITokenService
    {
        Task<TokenEntity> GetRefreshTokenByUserIdAsync(int userId);
        Task InsertRefreshTokenAsync(TokenEntity token);
        Task UpdateRefreshTokenByUserIdAsync(TokenEntity token);
    }
}