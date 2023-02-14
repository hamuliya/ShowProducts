using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WebAPI.Global.Middleware.Jwt.Interface;

namespace WebAPI.Global.Middleware.Jwt.Services
{
    public class JwtMiddleware : IJwtMiddleware
    {

        private readonly IConfiguration _configuration;
        public JwtMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, string issuer, string audience, DateTime expires, byte[] encodeKey, string algorithm = SecurityAlgorithms.HmacSha256)
        {

            if (expires <= DateTime.UtcNow)
            {
                throw new ArgumentException("The 'expires' parameter must be a date/time in the future.", "expires");
            }

            if (encodeKey == null || encodeKey.Length == 0)
            {
                throw new ArgumentException("The 'encodeKey' parameter must be a non-empty byte array.", "encodeKey");
            }

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


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string issuer, string audience, byte[] encodeKey, string token, string algorithm = SecurityAlgorithms.HmacSha256)
        {

            var jwtSection = _configuration.GetSection("Jwt");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(encodeKey),
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var claimsprincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(algorithm, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return claimsprincipal;
        }



        public bool ValidateAccessToken(string accessToken, string issuer, string audience, byte[] secretKey)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            try
            {
                var securityKey = new SymmetricSecurityKey(secretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(accessToken, validationParameters, out var securityToken);

                // Validation was successful. The decoded token contains the claims (i.e. user information).
                return true;
            }
            catch (SecurityTokenException)
            {
                // The token is invalid (e.g. signature is invalid, claims are invalid, etc.)
                return false;
            }
        }

    }
}
