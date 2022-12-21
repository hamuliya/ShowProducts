using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Global.Token
{
    public class Token : IToken
    {

        public string GenerateAccessToken(IEnumerable<Claim> claims, string issuer, string audience, DateTime expires, byte[] encodeKey, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            var secretKey = new SymmetricSecurityKey(encodeKey);
            var signinCredentials = new SigningCredentials(secretKey, algorithm);
            var tokeOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken(int numberLength = 64)
        {
            var randomNumber = new byte[numberLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string issuer, string audience, byte[] encodeKey, string token, bool validateAudience = true, bool validateIssuer = true,
            bool validateIssuerSigningKey = true, bool validateLifetime = true, string algorithm = SecurityAlgorithms.HmacSha256)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = validateAudience,
                ValidAudience = audience,
                ValidateIssuer = validateIssuer,
                ValidIssuer = issuer,
                ValidateIssuerSigningKey = validateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(encodeKey),
                ValidateLifetime = validateLifetime
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var claimsprincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(algorithm, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return claimsprincipal;
        }
    }
}
