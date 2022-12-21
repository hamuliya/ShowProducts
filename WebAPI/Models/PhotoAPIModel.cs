using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class PhotoAPIModel
{

    [Required]
    public int PhotoID { get; set; }
    [Required]
    public IFormFile? Photo { get; set; }
   
}
