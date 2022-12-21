﻿using Microsoft.AspNetCore.Authorization;
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
    public class TokenController : ControllerBase
    {
        private readonly IToken _token;
        private readonly IUserData _userData;
        private readonly IConfiguration _configuration;
        private readonly IEncode _encode;
        private readonly IRefreshTokenData _refreshTokenData;

        public TokenController(IToken token,IUserData userData,IConfiguration configuration,IEncode encode,IRefreshTokenData refreshTokenData)
        {
            _token = token;
            _userData = userData;
            _configuration = configuration;
            _encode = encode;
            _refreshTokenData = refreshTokenData;
        }


        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenModel tokenModel)
        {
            if (tokenModel is null)
                return BadRequest("Invalid client request");
            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
          
 
            var encodeKey = _encode.Encode(_configuration.GetSection("Jwt:Key").Value);


            var principal = _token.GetPrincipalFromExpiredToken(tokenModel.Issuer, tokenModel.Audience, encodeKey, accessToken);

            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var userDBTask = _userData.GetUserByName(username);

            var userDB = await userDBTask;


            if (userDB is null) return BadRequest("Invalid client request");


            var refreshTokenTask = _refreshTokenData.GetRefreshTokenByUserId(userDB.UserId);

            var refreshTokenDB = await refreshTokenTask;



            if (refreshTokenDB is null || refreshTokenDB.RefreshToken != refreshToken || refreshTokenDB.Expiry <= DateTime.Now)
                return BadRequest("Invalid client request");


            string? issuer = _configuration.GetSection("Jwt:Issuer").Value;
            string? audience = _configuration.GetSection("Jwt:Audience").Value;
            var expires = DateTime.Now.AddDays(7);

            var newAccessToken = _token.GenerateAccessToken(principal.Claims, issuer, audience, expires, encodeKey);

            var newRefreshToken = _token.GenerateRefreshToken();
            refreshTokenDB.RefreshToken = newRefreshToken;
            refreshTokenDB.Expiry = expires;
            refreshTokenDB.UserId = userDB.UserId;

            // Update refresh token

            await _refreshTokenData.UpdateRefreshTokenByUserId(refreshTokenDB);

            return Ok(new TokenModel()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
               

            });
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
