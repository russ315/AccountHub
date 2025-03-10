using AccountHub.Application.DTOs;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Abstractions;

public interface IUserService
{
    Task<UserEntity> Register(UserRegisterDto userRegisterDto);
    Task<UserEntity> Login(UserLoginDto userLoginDto);
    Task<UserEntity> RefreshToken(string username);
}