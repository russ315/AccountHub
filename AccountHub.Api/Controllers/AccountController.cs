﻿using AccountHub.Application.DTOs.Authentication;
using AccountHub.Application.Services.Abstractions;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AccountHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<ActionResult<UserEntity>> Register([FromForm] UserRegisterDto model)
    {
        if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            return BadRequest("You are already logged in"); 
        await _userService.Register(model,HttpContext.RequestAborted);
        
        return Ok("Please check your email and confirm your account");
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserEntity>> Login(UserLoginDto model)
    {
        if(HttpContext.User.Identity?.IsAuthenticated??false)
            return BadRequest("You are already logged in");
        var user = await _userService.Login(model);
        await AddTokenCookies(user);

        return Ok(user);
    }
    
    [HttpPost("assign-role")]
    [Authorize("Admin")]
    public async Task<ActionResult<UserEntity>> AssignRoles(RoleAssignDto model)
    {
        await _userService.AssignRole(model);
        
        return Ok();
    }
    
    [HttpPost("change-image")]
    [Authorize("Admin")]
    public async Task<ActionResult<UserEntity>> AssignImage([FromForm]UserChangeImageDto model)
    {
        await _userService.ChangeUserImage(model,HttpContext.RequestAborted);
        
        return Ok();
    }
    
    [HttpGet("get-roles")]
    [Authorize("Admin")]
    public async Task<ActionResult<UserEntity>> GetRoles()
    {
        var result = await _userService.GetAllRoles(HttpContext.RequestAborted);
        
        return Ok(result);
    }
    
    [HttpGet("logout")]
    [Authorize]
    public  Task<ActionResult> Logout()
    {
        Response.Cookies.Delete(AccessTokenCookieName);
        Response.Cookies.Delete(RefreshTokenCookieName);
        
        return Task.FromResult<ActionResult>(NoContent());
    }

    [HttpGet("refresh")]
    public async Task<ActionResult> RefreshJwtToken()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        if(refreshToken==null)
            throw new BadRequestException("Refresh token is invalid","Refresh token cookie is required");
        var accessToken = Request.Cookies[AccessTokenCookieName]!;
        var deviceId = Request.Headers[DeviceIdHeaderName];
        var refreshTokenIsValid = await _userService.CheckRefreshToken(refreshToken,accessToken,deviceId!,HttpContext.RequestAborted);
        if (!refreshTokenIsValid)
            throw new BadRequestException("Refresh token is invalid","Refresh token cookie is invalid.Please log in again");

        var user = await _userService.GetUserByAccessToken(accessToken);
        await AddTokenCookies(user);

        return Ok();
    }

    [HttpGet("confirm-email")] 
    public async Task<ActionResult<UserEntity>> ConfirmEmail(ConfirmEmailDto model)
    {

        if(await _userService.ConfirmEmailAddress(model))
            return Ok();
        return BadRequest();
        
    }
    
    [HttpGet("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        await _userService.ForgotPassword(email);
        
        return Ok("If an account exists, a password reset email has been sent.");
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
    {
        var result = await _userService.ResetPassword(model);
        if(result)
            return Ok();
        return BadRequest();
    }
    private async Task AddTokenCookies(UserEntity user)
    {
        var deviceId = Request.Headers[DeviceIdHeaderName].ToString();
        if(string.IsNullOrWhiteSpace(deviceId))
            throw new BadRequestException("Invalid device id","Device id header is required");
        var jwt =await _userService.GetAccessToken(user);
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