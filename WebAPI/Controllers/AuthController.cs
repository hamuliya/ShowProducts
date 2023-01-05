using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Global.Encode;
using WebAPI.Global.Hashing;
using WebAPI.Global.Token;
using WebAPI.Models.Auth;
using WebAPI.Models.Login;
using WebAPI.Models.Token;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHashing _hashing;
    private readonly IConfiguration _configuration;
    private readonly IToken _token;
    private readonly IEncode _encode;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService userService,IHashing hashing,IConfiguration configuration,IToken token,IEncode encode,ITokenService tokenService)
    {
        _userService = userService;
        _hashing = hashing;
        _configuration = configuration;
        _token = token;
        _encode = encode;
        _tokenService = tokenService;
    }



    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterModel userRegister)
    {
        //Check the UserName whether exists?

        var user = await _userService.GetUserByNameAsync(userRegister.UserName);
        if (user != null) return BadRequest("The user already exists.");
        
        
        var userDB = new UserEntity();

        userDB.UserName = userRegister.UserName;
        userDB.FirstName = userRegister.FirstName;
        userDB.LastName = userRegister.LastName;
        userDB.EmailAddress = userRegister.EmailAddress;
        userDB.CreateDate = userRegister.CreateDate;


        //Generate PasswordSalt
        userDB.Salt = _hashing.GenerateSalt(12);

        //Generate PasswordHash
        userDB.PasswordHash = _hashing.GeneratePasswordHash(userRegister.Password + userDB.Salt);

        //Insert User into DataBase
        try
        {
           int userId=await _userService.InsertUserAsync(userDB);
           if (userId ==null) return BadRequest("Not inserted ");
            
           //Generate RefreshToken
           var refreshToken=_token.GenerateRefreshToken();


           //Insert RefreshToken into Database
           var refreshTokenDB = new TokenEntity();
            
            refreshTokenDB.UserId = userId;
            refreshTokenDB.RefreshToken = refreshToken;
            refreshTokenDB.RefreshTokenExpiry = DateTime.Now.AddDays(7);


            await _tokenService.InsertTokenAsync(refreshTokenDB);


            return Ok("Inserted successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }



    [HttpPost("login")]
    public async Task<ActionResult> Login(UserLoginModel userLogin)
    {
        //check the userName whether exists
        
        var userDB=await _userService.GetUserByNameAsync(userLogin.UserName);

        if (userDB == null)
        {
            return BadRequest("User does not exist.");
        }
       

        //Verify PasswordHash
        var verified=_hashing.Verify(userLogin.Password+ userDB.Salt,userDB.PasswordHash);

        if (!verified)
        {
            return BadRequest("Wrong password.");
        }

        //Default role is Visitor
        var role = "Visitor";

        if (userDB.Role != null)
            role = userDB.Role;


        //issue a JWT to the user
        var token = CreateToken(userDB.UserName, role);

        var result=await RefreshToken(userDB.UserId);

        if (result)

        { return Ok(token); }

        else { return BadRequest("RefreshToken is not updated"); }

    }



    private async Task<bool>  RefreshToken(int userId)
    {
        //Generate RefreshToken       
        var refreshToken = _token.GenerateRefreshToken();

        // On the client

        // Set the refresh token in an HttpOnly cookie when the user logs in

        RefreshTokenModel refreshTokenModel = new RefreshTokenModel();
        refreshTokenModel.Expiry = DateTime.Now.AddDays(7);
        refreshTokenModel.Token = refreshToken;

       

        SetRefreshToken(refreshTokenModel);


        //Update RefreshToken in the Database
        var refreshTokenDB = new TokenEntity();


        refreshTokenDB.UserId = userId;
        refreshTokenDB.RefreshToken = refreshToken;
        refreshTokenDB.RefreshTokenExpiry = DateTime.Now.AddDays(7);
        bool result = false;
        try
        {
            await _tokenService.UpdateTokenByUserIdAsync(refreshTokenDB);
            result = true;
            return result;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return result;

        }
       

    }



    // On the client

    // Set the refresh token in an HttpOnly cookie when the user logs in
    private void SetRefreshToken(RefreshTokenModel newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expiry
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
    }




   


    //issue a JWT to the user
    private string CreateToken(string userName,string role)
    {

        var issuer = _configuration.GetSection("Jwt:Issuer").Value;
        var audience = _configuration.GetSection("Jwt:Audience").Value;
        var key = new SymmetricSecurityKey(_encode.Encode( _configuration.GetSection("Jwt:Key").Value));
        var expires=DateTime.Now.AddDays(1);

        List<Claim> claims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, userName),
          new Claim(ClaimTypes.Role,role)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
            (
              claims: claims,
              expires: expires,
              issuer: issuer,
              audience: audience,
              signingCredentials: creds

            );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;

    }

 


}
  
