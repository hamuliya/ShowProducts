using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.Global.Middleware.Jwt.Interface;
using WebAPI.Infrastructure.Encode.Interface;
using WebAPI.Infrastructure.Hashing.Interface;
using WebAPI.Infrastructure.Validation.Interface;
using WebAPI.Models.Auth;
using WebAPI.Models.Login;
using WebAPI.Models.Token;


namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHashing _hashing;
    private readonly IConfiguration _configuration;
    
    private readonly IEncode _encode;
    private readonly ITokenService _tokenService;
    private readonly IValidationFilter _validationFilter;
    private readonly IJwtMiddleware _jwt;

    public AuthController(IUserService userService,IHashing hashing,IConfiguration configuration,IEncode encode,ITokenService tokenService,IValidationFilter validationFilter,IJwtMiddleware jwt)
    {
        _userService = userService;
        _hashing = hashing;
        _configuration = configuration;
      
        _encode = encode;
        _tokenService = tokenService;
        _validationFilter = validationFilter;
        _jwt = jwt;
    }





    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterModel userRegister)
    {

        // Validate the username
        if (!_validationFilter.UserNameValidator(userRegister.UserName))
        {
            return BadRequest("Invalid username");
        }

        // Validate the password
        if (!_validationFilter.PasswordValidator(userRegister.Password))
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
           var refreshToken =_jwt.GenerateRefreshToken();


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
        if (!_validationFilter.UserNameValidator(userLogin.UserName))
        {
            return BadRequest("Invalid username");
        }

        // Validate the password
        if ( !_validationFilter.PasswordValidator(userLogin.Password))
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
        var expires = DateTime.Now.AddDays(7);

        var accessToken = _jwt.GenerateAccessToken(claims, issuer, audience, expires, encodeKey);

        var refreshToken = _jwt.GenerateRefreshToken();

    

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


            var principal = _jwt.GetPrincipalFromExpiredToken(tokenModel.Issuer, tokenModel.Audience, encodeKey, accessToken);


            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var userDB = await _userService.GetUserByNameAsync(username);

            if (userDB is null) return BadRequest("Invalid client request");


            var refreshTokenDB = await _tokenService.GetRefreshTokenByUserIdAsync(userDB.UserId);


            if (refreshTokenDB is null || refreshTokenDB.RefreshToken != refreshToken || refreshTokenDB.RefreshTokenExpiry <= DateTime.Now)
                return BadRequest("Invalid client request");


            string? issuer = _configuration.GetSection("Jwt:Issuer").Value;
            string? audience = _configuration.GetSection("Jwt:Audience").Value;
            var expires = DateTime.Now.AddDays(7);

            var newAccessToken = _jwt.GenerateAccessToken(principal.Claims, issuer, audience, expires, encodeKey);

            var newRefreshToken = _jwt.GenerateRefreshToken();
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






  

   





}

