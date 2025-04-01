using System.Text.Json;
using System.Text.Json.Serialization;
using AccountHub.Application.DTOs.Authentication;
using AccountHub.Application.Services.Abstractions;
using AccountHub.Application.Mapper;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Models;
using AccountHub.Domain.Options;
using AccountHub.Domain.Repositories;
using AccountHub.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AccountHub.Application.Services;

public class UserService : IUserService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IImageService _imageService;
    private readonly IMailjetService _mailSenderService;
    private readonly ClientAppOptions _clientAppOptions;
    public UserService(IJwtService jwtService, UserManager<UserEntity> userManager
        , RoleManager<IdentityRole> roleManager, SignInManager<UserEntity> signInManager,
        IRefreshTokenRepository refreshTokenRepository,IImageService imageService
        ,IMailjetService mailSenderService, IOptions<ClientAppOptions> clientAppOptions)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _refreshTokenRepository = refreshTokenRepository;
        _imageService = imageService;
        _mailSenderService = mailSenderService;
        _clientAppOptions = clientAppOptions.Value;
    }
   

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

   


    public async Task<UserEntity> Register(UserRegisterDto userRegisterDto,CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(userRegisterDto.Email) != null)
            throw new DuplicateEntityException("Duplicate entity","Email address already exists");
        var userEntity = userRegisterDto.ToEntity();
        var userCreateResult = await _userManager.CreateAsync(userEntity, userRegisterDto.Password);
        if (!userCreateResult.Succeeded)
        {
            throw new BadRequestException("Invalid data", "See errors")
            {
                Details = userCreateResult.Errors
            };
        }
        await SendConfirmEmail(userEntity);

        if (userRegisterDto.Image != null)
        {
            var fileName = userRegisterDto.Username + userRegisterDto.Email;
            userEntity.UpdateProfile(await _imageService.UploadImage(fileName,userRegisterDto.Image.OpenReadStream(),cancellationToken));
        }
        
        return userEntity;
    }

    public async Task<UserEntity> Login(UserLoginDto userLoginDto)
    {
        var userEntity = await _userManager.FindByNameAsync(userLoginDto.Username);
        if (userEntity == null)
            throw new BadRequestException("Invalid data", "Invalid username or password");
        if (!await _userManager.IsEmailConfirmedAsync(userEntity))
            throw new ForbiddenException("Email is not confirmed",
                $"Please confirm your email {userEntity.Email} and try again");

        var result = await _signInManager.PasswordSignInAsync(userEntity, userLoginDto.Password, false, false);
        if (!result.Succeeded)
            throw new BadRequestException("Invalid data", "Invalid username or password");
        return userEntity;
    }

    public async Task<string> GetAccessToken(UserEntity userEntity)
    {
        var jwt = await _jwtService.GenerateJwtAccessToken(userEntity);
        return jwt;
    }

    public async Task<UserEntity> GetUserByAccessToken(string accessToken)
    {
        var userId = await _jwtService.GetUserIdFromExpiredToken(accessToken);
        var user = await _userManager.FindByIdAsync(userId);

        return user!;
    }

    public async Task<bool> CheckRefreshToken(string refreshToken, string accessToken, string deviceId,
        CancellationToken cancellationToken)
    {
        var userId = await _jwtService.GetUserIdFromExpiredToken(accessToken);
        var refreshTokenEntity = await _refreshTokenRepository.GetRefreshToken(userId, deviceId, cancellationToken);
        if (refreshTokenEntity?.Token != refreshToken || refreshTokenEntity.Expires < DateTime.Now)
            return false;

        return true;
    }

    public async Task<bool> AssignRole(RoleAssignDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            throw new EntityNotFoundException("Invalid data", "Invalid username");
        var roleAddResult = await _userManager.AddToRoleAsync(user, model.RoleName);
        if (!roleAddResult.Succeeded)
            throw new BadRequestException("Invalid data", "See errors")
            {
                Details = roleAddResult.Errors
            };
        return true;
    }

    public async Task<bool> ConfirmEmailAddress(ConfirmEmailDto model)
    {
        var decodedToken = Uri.UnescapeDataString(model.Token);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if(user is null)
            throw new EntityNotFoundException("Invalid data", "Invalid email");
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if(!result.Succeeded)
            throw new BadRequestException("Invalid data", "Invalid token")
            {
                Details = result.Errors
            };
        await _userManager.AddToRoleAsync(user, RoleConstants.User);
        await _signInManager.SignInAsync(user, false);
        return true;
    }

   
    public async Task<string> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return default;
        await SendResetPasswordEmail(user);
        return default;
    }

    public async Task<bool> ResetPassword(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if(user is null)
            throw new EntityNotFoundException("Invalid data", "Invalid email");
        var decodedToken = Uri.UnescapeDataString(model.Token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken,model.NewPassword);
        if(!result.Succeeded)
            throw new BadRequestException("Invalid data", "Invalid token")
            {
                Details = result.Errors
            };
        return true;
    }

    public async Task<List<IdentityRole>> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles =await _roleManager.Roles.ToListAsync(cancellationToken);
        return roles;
    }

    public async Task<string> ChangeUserImage(UserChangeImageDto userChangeImageDto,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userChangeImageDto.UserName);
        if(user is null)
            throw new EntityNotFoundException("Invalid data", "Invalid username");
        var fileName = user.UserName + user.Email;
        var url = await _imageService.UploadImage(fileName, userChangeImageDto.Image.OpenReadStream(),cancellationToken);
        user.UpdateProfile(url);
        await _userManager.UpdateAsync(user);
        return url;
    }

    private async Task<string> SendConfirmEmail(UserEntity userEntity)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
        var parameters = new Dictionary<string, object>();
        parameters.Add("username", userEntity.UserName!);
        parameters.Add("url", $"{_clientAppOptions.Url}/confirm-email?email={userEntity.Email}&token={Uri.EscapeDataString(token)}");
        return await _mailSenderService.SendEmailAsync(userEntity.Email!,"Confirm email", TemplateIdConstants.EmailConfirmationTemplate,parameters);

    }
    private async Task<string> SendResetPasswordEmail(UserEntity userEntity)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(userEntity);
        var parameters = new Dictionary<string, object>();
        parameters.Add("username", userEntity.UserName!);
        parameters.Add("url", $"{_clientAppOptions.Url}/reset-password?email={userEntity.Email}&token={Uri.EscapeDataString(token)}");
        return await _mailSenderService.SendEmailAsync(userEntity.Email!,"Confirm email", TemplateIdConstants.ResetPasswordTemplate,parameters);

    }
}