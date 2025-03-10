using System.Security.Authentication;
using AccountHub.Application.DTOs;
using AccountHub.Application.Services.Abstractions;
using AccountHub.Application.Mapper;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Models;
using AccountHub.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Application.Services;

public class UserService:IUserService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<UserEntity> _signInManager;

    public UserService(IJwtService jwtService,UserManager<UserEntity> userManager
        ,RoleManager<IdentityRole> roleManager,SignInManager<UserEntity> signInManager)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    public async Task<UserEntity> Register(UserRegisterDto userRegisterDto)
    {
        if(await _userManager.FindByEmailAsync(userRegisterDto.Email) != null)
            throw new AuthenticationException("Email already exists");
        
        var userEntity = userRegisterDto.ToEntity();
        var userCreateResult =await _userManager.CreateAsync(userEntity, userRegisterDto.Password);
        if(!userCreateResult.Succeeded)
            throw new InvalidCredentialException("Registering user failed.");
        await  _userManager.AddToRoleAsync(userEntity,RoleConsts.User);
        await _signInManager.SignInAsync(userEntity, false);
        return userEntity;

    }

    public async Task<UserEntity> Login(UserLoginDto userLoginDto)
    {
        var userEntity = await _userManager.FindByNameAsync(userLoginDto.Username);
        if(userEntity == null)
            throw new InvalidCredentialException("Invalid username or password");
        if(!await _userManager.IsEmailConfirmedAsync(userEntity))
            throw new AuthenticationException("Email is not confirmed");
        
        var result = await _signInManager.PasswordSignInAsync(userEntity,userLoginDto.Password,false,false);
        if(!result.Succeeded)
            throw new InvalidCredentialException("Invalid username or password");
        return userEntity;    
    }

    
    
    public async Task<UserEntity> RefreshToken(string username)
    {
        throw new NotImplementedException();


    }

}