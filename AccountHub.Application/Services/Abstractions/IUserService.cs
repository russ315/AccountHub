using AccountHub.Application.DTOs;
using AccountHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Application.Services.Abstractions;

public interface IUserService
{
    Task<UserEntity> Register(UserRegisterDto userRegisterDto);
    Task<UserEntity> Login(UserLoginDto userLoginDto);
    Task<string> GetRefreshToken(string accessToken,string userId);
    Task<string> GetAccessToken(UserEntity userEntity);
    Task<UserEntity> GetUserByAccessToken(string accessToken);
    Task<bool> CheckRefreshToken(string refreshToken,string accessToken,string deviceId);
    Task<bool> AssignRole(RoleAssignDto model);
    Task<List<IdentityRole>> GetAllRoles();
}