using AccountHub.Application.DTOs.Authentication;
using AccountHub.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace AccountHub.Application.Services.Abstractions;

public interface IUserService
{
    Task<string> GetRefreshToken(string accessToken,string deviceId);
    Task<string> GetAccessToken(UserEntity userEntity);
    Task<UserEntity> GetUserByAccessToken(string accessToken);
    Task<bool> CheckRefreshToken(string refreshToken, string accessToken, string deviceId,
        CancellationToken cancellationToken);
    Task<List<IdentityRole>> GetAllRoles(CancellationToken cancellationToken);
    Task<string> ChangeUserImage(UserChangeImageDto userChangeImageDto, CancellationToken cancellationToken);
    Task<UserEntity> Register(UserRegisterDto userRegisterDto, CancellationToken cancellationToken);
    Task<UserEntity> Login(UserLoginDto userLoginDto);
    Task<bool> AssignRole(RoleAssignDto model);
}