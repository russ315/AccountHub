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
    private readonly IJwtService _jwtService;

    public AccountController(IUserService userService,IJwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserEntity>> Register(UserRegisterDto model)
    {
        var user = await _userService.Register(model);
        var jwt =_jwtService.GenerateJwtAccessToken(user);
        HttpContext.Response.Cookies.Append("access_token",jwt);
        return Ok(user);
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserEntity>> Login(UserLoginDto model)
    {
        var user = await _userService.Login(model);
        var jwt =_jwtService.GenerateJwtAccessToken(user);
        HttpContext.Response.Cookies.Append("access_token",jwt);
        return Ok(user);
    }

    [HttpGet("refresh")]
    public async Task<ActionResult<UserEntity>> RefreshJwtToken()
    {
        Console.Write(HttpContext.Request.Cookies[".AspNetCore.Identity.Application"]);
        HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken);
        var user = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        if(user==null)
            throw new UnauthorizedAccessException();
        var jwt = await _userService.RefreshToken(user);
        return Ok(jwt);
    }
    
}