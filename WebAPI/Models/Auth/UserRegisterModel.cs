using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Auth;

public class UserRegisterModel
{
    [Required]
    [MaxLength(50)]
    public string? UserName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Password { get; set; }


    [Required]
    [MaxLength(100)]
    public string? EmailAddress { get; set; }

    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    [Required]
    public DateTime CreateDate { get; set; }

}
