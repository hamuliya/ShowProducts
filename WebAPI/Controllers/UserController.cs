using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI;


namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userData;
  

    public UserController(IUserService userData)
    {
        _userData = userData;
      
    }


    [HttpGet]

    public async Task<IResult> GetUserByName(string userName)
    {
        try
        {
            var user= await _userData.GetUserByName(userName);
            if (user == null) return Results.NotFound();
            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return (Results.Problem(ex.Message));
        }
    }


   



   

}
