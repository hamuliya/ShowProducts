using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

public class UserEntity
{

    [Key]
    [Required]
    public int userId { get; set; }

    [Required]
    [MaxLength(50)]
    public string? userName { get; set; }
    [Required]
    [MaxLength(300)]
    public string? passwordHash { get; set; }

    [Required]
    [MaxLength(50)]
    public string? salt { get; set; }

    [Required]
    [MaxLength(100)]
    public string? emailAddress { get; set; }

    public int roleId { get; set;}

    [Required]
    [MaxLength(50)]
    public string? firstName { get; set;}
    [Required]
    [MaxLength(50)]
    public string? lastName { get; set;}

    [Required]
    public DateTime createDate { get; set;}

    public string? role { get; }


   

}
