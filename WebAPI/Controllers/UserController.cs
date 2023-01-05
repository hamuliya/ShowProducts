using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI;


namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
  

    public UserController(IUserService userService)
    {
        _userService = userService;
      
    }


    [HttpGet]

    public async Task<IResult> GetUserByName(string userName)
    {
        try
        {
            var user= await _userService.GetUserByNameAsync(userName);
            if (user == null) return Results.NotFound();
            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }


   



   

}
