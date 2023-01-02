using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenEntity> GetRefreshTokenByUserIdAsync(int userId);
        Task InsertRefreshTokenAsync(RefreshTokenEntity refreshToken);
        Task UpdateRefreshTokenByUserIdAsync(RefreshTokenEntity refreshToken);
    }
}