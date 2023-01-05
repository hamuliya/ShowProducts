using DataAccess.Models;

namespace DataAccess.Data
{
    public interface ITokenService
    {
        Task<TokenEntity> GetTokenByUserIdAsync(int userId);
        Task InsertTokenAsync(TokenEntity token);
        Task UpdateTokenByUserIdAsync(TokenEntity token);
    }
}