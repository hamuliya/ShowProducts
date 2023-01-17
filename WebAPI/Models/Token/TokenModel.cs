using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Token
{
    public class TokenModel
    {

        public int UserId { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }

        public DateTime AccessTokenExpiry { get; set; }

    }
}
