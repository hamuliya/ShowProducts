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

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserData _userData;
    private readonly IHashing _hashing;
    private readonly IConfiguration _configuration;
    private readonly IToken _token;
    private readonly IEncode _encode;
    private readonly IRefreshTokenData _refreshTokenData;

    public AuthController(IUserData userData,IHashing hashing,IConfiguration configuration,IToken token,IEncode encode,IRefreshTokenData refreshTokenData)
    {
        _userData = userData;
        _hashing = hashing;
        _configuration = configuration;
        _token = token;
        _encode = encode;
        _refreshTokenData = refreshTokenData;
    }



    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterModel userRegister)
    {
        //Check the UserName whether exists?

        var user = await _userData.GetUserByName(userRegister.UserName);
        if (user != null) return BadRequest("The user already exists.");
        //

        var userDB = new UserDBModel();

        userDB.UserName = userRegister.UserName;
        userDB.FirstName = userRegister.FirstName;
        userDB.LastName = userRegister.LastName;
        userDB.EmailAddress = userRegister.EmailAddress;
        userDB.CreateDate = userRegister.CreateDate;


        //Generate PasswordSalt
        userDB.Salt = _hashing.GenerateSalt(12);



        //Generate PasswordHash

        userDB.PasswordHash = _hashing.GeneratePasswordHash(userRegister.Password + userDB.Salt);

       




        //Insert into DataBase
        try
        {
           int userId= _userData.InsertUser(userDB);
           if (userId ==null) return BadRequest("Not inserted ");
            
           //Generate RefreshToken
           var refreshToken=_token.GenerateRefreshToken();

           var refreshTokenDB = new RefreshTokenDBModel();
            
            refreshTokenDB.UserId = userId;
            refreshTokenDB.RefreshToken = refreshToken;

            //need the same?
            refreshTokenDB.Expiry = DateTime.Now.AddDays(7);


            await _refreshTokenData.InsertRefreshToken(refreshTokenDB);


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
        //check the userName whether exists?
        var userDB=await _userData.GetUserByName(userLogin.UserName);


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

        var role = "Visitor";

        //Create Token
        if (userDB.Role != null)
            role = userDB.Role;   
        

        var token= CreateToken(userDB.UserName, role);

        //Generate RefreshToken



        //Set RefreshToken


        return Ok(token);

    }

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
  
