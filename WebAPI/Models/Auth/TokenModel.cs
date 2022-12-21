using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Auth
{
    public class TokenModel
    {
       // [Required]
       // public int UserId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? AccessToken { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? RefreshToken { get; set; }

        [Required]
        public String? Issuer { get; set; }

        [Required]
        public String? Audience { get; set; }

        //[Required]
        //public DateTime Expiry { get; set; }


       


    }
}
