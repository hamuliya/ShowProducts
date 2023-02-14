using System.Security.Claims;

namespace WebAPI.Global.Middleware.Jwt.Interface
{
    public interface IJwtMiddleware
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, string issuer, string audience, DateTime expires, byte[] encodeKey, string algorithm = "HS256");
        string GenerateRefreshToken(int numberLength = 64);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string issuer, string audience, byte[] encodeKey, string token, string algorithm = "HS256");
        bool ValidateAccessToken(string accessToken, string issuer, string audience, byte[] secretKey);
    }
}