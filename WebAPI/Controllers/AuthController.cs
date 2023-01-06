using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        // Validate the username
        if (!UserNameValidator(userRegister.UserName))
        {
            return BadRequest("Invalid username");
        }

        // Validate the password
        if (!PasswordValidator(userRegister.Password))
        {
            return BadRequest("Invalid password");
        }


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


            await _tokenService.InsertRefreshTokenAsync(refreshTokenDB);


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

        // Validate the username
        if (!UserNameValidator(userLogin.UserName))
        {
            return BadRequest("Invalid username");
        }

        // Validate the password
        if ( !PasswordValidator(userLogin.Password))
        {
            return BadRequest("Invalid password");
        }


        //Check the userName whether exists

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


        List<Claim> claims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, userDB.UserName),
          new Claim(ClaimTypes.Role,role),
        };


        var jwtSection = _configuration.GetSection("Jwt");

   
        var issuer = jwtSection.GetValue<string>("Issuer");
        var audience = jwtSection.GetValue<string>("Audience");
        var encodeKey =_encode.Encode(jwtSection.GetValue<string>("Key"));
        var expires = DateTime.Now.AddDays(1);

        var accessToken = _token.GenerateAccessToken(claims, issuer, audience, expires, encodeKey);

        var refreshToken = _token.GenerateRefreshToken();

    

        //Update RefreshToken in the Database
        var refreshTokenDB = new TokenEntity();


        refreshTokenDB.UserId = userDB.UserId;
        refreshTokenDB.RefreshToken = refreshToken;
        refreshTokenDB.RefreshTokenExpiry = DateTime.Now.AddDays(7);
        bool result = false;
        try
        {
            await _tokenService.UpdateRefreshTokenByUserIdAsync(refreshTokenDB);
            result = true;
           

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


        if (result)

        {
            // Set the refresh token in an HttpOnly cookie when the user logs in

            SetClientToken("RefreshToken", refreshToken, 7);

            SetClientToken("AccessToken", accessToken, 1);


            return Ok(accessToken); 
        
        }

        else { return BadRequest("RefreshToken is not updated"); }

    }



    [HttpPost]
    [Route("RefreshToken")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        try
        {
           

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            // Validate the access token
            if (string.IsNullOrEmpty(accessToken) || accessToken.Length < 50)
            {
                return BadRequest("Invalid access token");
            }

            // Validate the refresh token
            if (string.IsNullOrEmpty(refreshToken) || refreshToken.Length < 50)
            {
                return BadRequest("Invalid refresh token");
            }



            var encodeKey = _encode.Encode(_configuration.GetSection("Jwt:Key").Value);


            var principal = _token.GetPrincipalFromExpiredToken(tokenModel.Issuer, tokenModel.Audience, encodeKey, accessToken);


            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var userDB = await _userService.GetUserByNameAsync(username);

            if (userDB is null) return BadRequest("Invalid client request");


            var refreshTokenDB = await _tokenService.GetRefreshTokenByUserIdAsync(userDB.UserId);


            if (refreshTokenDB is null || refreshTokenDB.RefreshToken != refreshToken || refreshTokenDB.RefreshTokenExpiry <= DateTime.Now)
                return BadRequest("Invalid client request");


            string? issuer = _configuration.GetSection("Jwt:Issuer").Value;
            string? audience = _configuration.GetSection("Jwt:Audience").Value;
            var expires = DateTime.Now.AddDays(7);

            var newAccessToken = _token.GenerateAccessToken(principal.Claims, issuer, audience, expires, encodeKey);

            var newRefreshToken = _token.GenerateRefreshToken();
            refreshTokenDB.RefreshToken = newRefreshToken;
            refreshTokenDB.RefreshTokenExpiry = expires;
            refreshTokenDB.UserId = userDB.UserId;

            // Update refresh token to database

            await _tokenService.UpdateRefreshTokenByUserIdAsync(refreshTokenDB);

            // Set the refresh token in an HttpOnly cookie when the user logs in

            SetClientToken("RefreshToken", newRefreshToken, 7);

            SetClientToken("AccessToken", newAccessToken, 1);


            return Ok(new TokenModel()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }



    }


   

    // On the client
    // Set the refresh token or access token in an HttpOnly cookie when the user logs in
    private void SetClientToken(string tokenName, string token, int expiryDays)
    {
        ClientTokenModel newToken = new ClientTokenModel();
        newToken.Expiry = DateTime.Now.AddDays(expiryDays);
        newToken.Token = token;


        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newToken.Expiry
        };
        Response.Cookies.Append(tokenName, newToken.Token, cookieOptions);
    }






    private static bool UserNameValidator(string userName)
    {

        bool result = false;
        if (string.IsNullOrEmpty(userName) || userName.Length < 3 || userName.Length > 50)
        {
            return result;
        }
        result = true;
        return result;

    }

        private static bool PasswordValidator(string password)
    {

        bool result = false;

        //Check if the password length is between 8 and 50
        if (string.IsNullOrEmpty(password) || password.Length < 8 ||password.Length > 50)
            return result;

        // Check if the password includes at least one capital letter
        bool hasCapitalLetter = false;
        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasCapitalLetter = true;
                break;
            }
        }


        if (!hasCapitalLetter)
        {
            Console.WriteLine("Password must include at least one capital letter");
            return result;
        }

        // Check if the password includes at least one special character
        bool hasSpecialCharacter = false;
        foreach (char c in password)
        {
            if (!char.IsLetterOrDigit(c))
            {
                hasSpecialCharacter = true;
                break;
            }
        }

        if (!hasSpecialCharacter)
        {
            Console.WriteLine("Password must include at least one special character");
            return result;
        }


       
        result=true;
       

        return result;

    }


   





}

