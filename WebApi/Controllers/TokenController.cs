using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebAPI.Global.Encode;
using WebAPI.Global.Token;
using WebAPI.Models.Token;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IToken _token;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IEncode _encode;
        private readonly ITokenService _tokenService;

        public TokenController(IToken token,IUserService userService,IConfiguration configuration,IEncode encode,ITokenService tokenService)
        {
            _token = token;
            _userService = userService;
            _configuration = configuration;
            _encode = encode;
            _tokenService = tokenService;
        }


        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                if ((tokenModel is null)||(tokenModel.AccessToken is null) || (tokenModel.RefreshToken is null))

                        return BadRequest("Invalid client request,please fill in the AccessToken and RefreshToken");

                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;


                var encodeKey = _encode.Encode(_configuration.GetSection("Jwt:Key").Value);


                var principal = _token.GetPrincipalFromExpiredToken(tokenModel.Issuer, tokenModel.Audience, encodeKey, accessToken);

                
                var username = principal.Identity.Name; //this is mapped to the Name claim by default

                var userDB =await _userService.GetUserByNameAsync(username);

                if (userDB is null) return BadRequest("Invalid client request");


                var refreshTokenDB =await _tokenService.GetTokenByUserIdAsync(userDB.UserId);

                



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

                await _tokenService.UpdateTokenByUserIdAsync(refreshTokenDB);

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






        //[HttpPost, Authorize]
        //[Route("revoke")]
        //public IActionResult Revoke()
        //{
        //    var username = User.Identity.Name;


        //    var user =_userDB.GetUserByName(username);
        //    if (user == null) return BadRequest();
        //    user.RefreshToken = null;
        //    _userContext.SaveChanges();
        //    return NoContent();
        //}


    }
}
