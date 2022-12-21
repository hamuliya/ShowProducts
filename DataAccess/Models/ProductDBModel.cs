using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;
public class ProductDBModel
{
    [Key]
    [Required]
    public int ProductId { get; set; }


    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }
    

    [Required]
    public DateTime UploadDate { get; set; }
    
    [Required]
    [MaxLength(4000)]
    public string? Detail { get; set; }

}
