using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Interfaces;

public interface IUserRepository
{
    Task<UserEntity> Register(UserEntity user,string password);
    Task<UserEntity> Login(UserEntity user,string password);
}