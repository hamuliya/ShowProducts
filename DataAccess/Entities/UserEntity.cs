using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities;

public class UserEntity
{

    [Key]
    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string? UserName { get; set; }
    [Required]
    [MaxLength(300)]
    public string? PasswordHash { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Salt { get; set; }

    [Required]
    [MaxLength(100)]
    public string? EmailAddress { get; set; }

    public int RoleId { get; set;}

    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set;}
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set;}

    [Required]
    public DateTime CreateDate { get; set;}

    public string? Role { get; }


   

}
