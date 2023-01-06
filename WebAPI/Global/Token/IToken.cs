using System.Security.Claims;

namespace WebAPI.Global.Token
{
    public interface IToken
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, string issuer, string audience, DateTime expires, byte[] encodeKey, string algorithm = "RS256");
        string GenerateRefreshToken(int numberLength = 64);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string issuer, string audience, byte[] encodeKey, string token, string algorithm = "RS256");
    }
}