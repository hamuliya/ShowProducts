using DataAccess.Models;

namespace DataAccess.Data
{
    public interface IRefreshTokenData
    {
        Task<RefreshTokenDBModel> GetRefreshTokenByUserId(int userId);
        Task InsertRefreshToken(RefreshTokenDBModel refreshToken);
        Task UpdateRefreshTokenByUserId(RefreshTokenDBModel refreshToken);
    }
}