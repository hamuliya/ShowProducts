using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Login;

public class UserLoginModel
{

    [Required]
    [MaxLength(50)]
    public string? UserName { get; set; }

    [Required]

    [MaxLength(50)]
    public string? Password { get; set; }

    //public string? RefreshToken { get; set; }
    //public DateTime RefreshTokenExpiryTime { get; set; }
}
