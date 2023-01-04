using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using WebAPI.Global.Encode;
using WebAPI.Global.Token;
using WebAPI.Models.Auth;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IToken _token;
        private readonly IUserService _userData;
        private readonly IConfiguration _configuration;
        private readonly IEncode _encode;
        private readonly IRefreshTokenService _refreshTokenData;

        public RefreshTokenController(IToken token,IUserService userData,IConfiguration configuration,IEncode encode,IRefreshTokenService refreshTokenData)
        {
            _token = token;
            _userData = userData;
            _configuration = configuration;
            _encode = encode;
            _refreshTokenData = refreshTokenData;
        }


        //??

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                if ((tokenModel is null)||(tokenModel.AccessToken is null) || (tokenModel.RefreshToken is null))

                        return BadRequest("Invalid client request");

                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;


                var encodeKey = _encode.Encode(_configuration.GetSection("Jwt:Key").Value);


                var principal = _token.GetPrincipalFromExpiredToken(tokenModel.Issuer, tokenModel.Audience, encodeKey, accessToken);

                var username = principal.Identity.Name; //this is mapped to the Name claim by default

                var userDB =await _userData.GetUserByNameAsync(username);

               


                if (userDB is null) return BadRequest("Invalid client request");


                var refreshTokenDB =await _refreshTokenData.GetRefreshTokenByUserIdAsync(userDB.userId);

                



                if (refreshTokenDB is null || refreshTokenDB.refreshToken != refreshToken || refreshTokenDB.expiry <= DateTime.Now)
                    return BadRequest("Invalid client request");


                string? issuer = _configuration.GetSection("Jwt:Issuer").Value;
                string? audience = _configuration.GetSection("Jwt:Audience").Value;
                var expires = DateTime.Now.AddDays(7);

                var newAccessToken = _token.GenerateAccessToken(principal.Claims, issuer, audience, expires, encodeKey);

                var newRefreshToken = _token.GenerateRefreshToken();
                refreshTokenDB.refreshToken = newRefreshToken;
                refreshTokenDB.expiry = expires;
                refreshTokenDB.userId = userDB.userId;

                // Update refresh token

                await _refreshTokenData.UpdateRefreshTokenByUserIdAsync(refreshTokenDB);

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
