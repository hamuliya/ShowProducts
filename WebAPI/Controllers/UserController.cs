using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI;


namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService,ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }



    [HttpGet]
    public async Task<IActionResult> GetUserByName(string userName)
    {
        // Validate userName
        if (string.IsNullOrWhiteSpace(userName))
            return BadRequest("userName is required.");

        try
        {
            var user = await _userService.GetUserByNameAsync(userName);
            if (user == null) return NotFound("userName not found.");
            return Ok(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "An error occurred while getting user name.");
            return StatusCode(500, "An error occurred while getting user name.");
        }
    }
}
