using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

public class RefreshTokenEntity
{
    [Required]
    
    public int userId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string? refreshToken { get; set; }

    [Required]
    public DateTime expiry { get; set; }
}
