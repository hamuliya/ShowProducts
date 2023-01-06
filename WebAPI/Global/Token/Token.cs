
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Global.Encode;
using WebAPI.Models.Token;


namespace WebAPI.Global.Token
{
    public class Token : IToken
    {
        private readonly IConfiguration _configuration;

        public Token(IConfiguration configuration)
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
                ValidateIssuer = jwtSection.GetValue<bool>("ValidateIssuer"),
                ValidateAudience = jwtSection.GetValue<bool>("ValidateAudience"),
                ValidateLifetime = jwtSection.GetValue<bool>("ValidateLifetime"),
                ValidateIssuerSigningKey = jwtSection.GetValue<bool>("ValidateIssuerSigningKey"),
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




    }
}


