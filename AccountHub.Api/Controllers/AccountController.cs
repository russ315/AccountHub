using System.Security.Authentication;
using System.Security.Claims;
using AccountHub.Application.DTOs;
using AccountHub.Application.Services.Abstractions;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController:ControllerBase
{
    private readonly IUserService _userService;
    private const string RefreshTokenCookieName = "refresh_token";
    private const string AccessTokenCookieName = "access_token";
    private const string DeviceIdHeaderName = "X-DeviceId";
    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserEntity>> Register(UserRegisterDto model)
    {
        var user = await _userService.Register(model);
        await AddTokenCookies(user);
        return Ok(user);
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserEntity>> Login(UserLoginDto model)
    {
        var user = await _userService.Login(model);
        await AddTokenCookies(user);

        return Ok(user);
    }

    [HttpGet("refresh")]
    public async Task<ActionResult<UserEntity>> RefreshJwtToken()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        if(refreshToken==null)
            throw new UnauthorizedAccessException();
        var accessToken = Request.Cookies[AccessTokenCookieName]!;
        var deviceId = Request.Headers[DeviceIdHeaderName];
        var refreshTokenIsValid = await _userService.CheckRefreshToken(refreshToken,accessToken,deviceId!);
        if (!refreshTokenIsValid)
            return Unauthorized();

        var user = await _userService.GetUserByAccessToken(accessToken);
        await AddTokenCookies(user);

        return Ok();
    }


    private async Task AddTokenCookies(UserEntity user)
    {
        var deviceId = Request.Headers[DeviceIdHeaderName].ToString();
        if(string.IsNullOrEmpty(deviceId))
            throw new InvalidCredentialException("Invalid device id");
        var jwt =_userService.GetAccessToken(user);
        var refreshToken =await _userService.GetRefreshToken(jwt,deviceId);

        Response.Cookies.Append(AccessTokenCookieName, jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(7)
        });
        
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(7)
        });
    }
    
}