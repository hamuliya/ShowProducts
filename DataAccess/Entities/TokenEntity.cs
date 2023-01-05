using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

public class TokenEntity
{
    [Required]
    
    public int UserId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string? RefreshToken { get; set; }

    [Required]
    public DateTime RefreshTokenExpiry { get; set; }


    [Required]
    [MaxLength(1000)]
    public string? AccessToken { get; set; }

    [Required]
    public DateTime AccessTokenExpiry { get; set; }
}

