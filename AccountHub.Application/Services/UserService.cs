using AccountHub.Application.DTOs;
using AccountHub.Application.Services.Abstractions;
using AccountHub.Application.Mapper;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Models;
using AccountHub.Domain.Repositories;
using AccountHub.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Application.Services;

public class UserService : IUserService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public async Task<string> GetRefreshToken(string accessToken, string deviceId)
    {
        var token = _jwtService.GenerateRefreshToken();
        var userId = await _jwtService.GetUserIdFromExpiredToken(accessToken);

        await _refreshTokenRepository.DeleteRefreshToken(userId, deviceId);


        
        await _refreshTokenRepository.AddRefreshToken(new RefreshTokenEntity()
        {
            UserId = userId,
            Token = token,
            DeviceId = deviceId,
            Expires = DateTime.UtcNow.AddDays(7)
        });
        return token;


    }

    private readonly SignInManager<UserEntity> _signInManager;

    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public UserService(IJwtService jwtService, UserManager<UserEntity> userManager
        , RoleManager<IdentityRole> roleManager, SignInManager<UserEntity> signInManager,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<UserEntity> Register(UserRegisterDto userRegisterDto)
    {
        if (await _userManager.FindByEmailAsync(userRegisterDto.Email) != null)
            throw new DuplicateEntityException("User is already registered",$"User with email {userRegisterDto.Email} already registered");

        var userEntity = userRegisterDto.ToEntity();
        var userCreateResult = await _userManager.CreateAsync(userEntity, userRegisterDto.Password);
        if (!userCreateResult.Succeeded)
            throw new BadRequestException("Invalid data","Registering user failed");
        await _userManager.AddToRoleAsync(userEntity, RoleConsts.User);
        await _signInManager.SignInAsync(userEntity, false);
        return userEntity;

    }

    public async Task<UserEntity> Login(UserLoginDto userLoginDto)
    {
        var userEntity = await _userManager.FindByNameAsync(userLoginDto.Username);
        if (userEntity == null)
            throw new BadRequestException("Invalid data","Invalid username or password");
        if (!await _userManager.IsEmailConfirmedAsync(userEntity))
            throw new ForbiddenException("Email is not confirmed",$"Please confirm your email {userEntity.Email} and try again");

        var result = await _signInManager.PasswordSignInAsync(userEntity, userLoginDto.Password, false, false);
        if (!result.Succeeded)
            throw new BadRequestException("Invalid data","Invalid username or password");
        return userEntity;
    }

    public string GetAccessToken(UserEntity userEntity)
    {
        var jwt = _jwtService.GenerateJwtAccessToken(userEntity);
        return jwt;
    }
    public async Task<UserEntity> GetUserByAccessToken(string accessToken)
    {
        var userId = await _jwtService.GetUserIdFromExpiredToken(accessToken);
        var user = await _userManager.FindByIdAsync(userId);
        
        return user!;
    }

    public async Task<bool> CheckRefreshToken(string refreshToken, string accessToken, string deviceId)
    {
        
        var userId = await _jwtService.GetUserIdFromExpiredToken(accessToken);
        var refreshTokenEntity = await _refreshTokenRepository.GetRefreshToken(userId, deviceId);
        if (refreshTokenEntity?.Token != refreshToken || refreshTokenEntity.Expires<DateTime.Now)
            return false;
        
        return true;

    }
}

