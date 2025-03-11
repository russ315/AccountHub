using AccountHub.Application.DTOs;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Abstractions;

public interface IUserService
{
    Task<UserEntity> Register(UserRegisterDto userRegisterDto);
    Task<UserEntity> Login(UserLoginDto userLoginDto);
    Task<string> GetRefreshToken(string accessToken,string userId);
    string GetAccessToken(UserEntity userEntity);
    Task<UserEntity> GetUserByAccessToken(string accessToken);
    Task<bool> CheckRefreshToken(string refreshToken,string accessToken,string deviceId);

}