using AccountHub.Domain.Entities;
using AccountHub.Domain.Interfaces;
using AccountHub.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Infrastructure.Repositories;

public class UserRepository:IUserRepository
{
    

    public UserRepository()
    {
       
    }
    public Task<UserEntity> Register(UserEntity user, string password)
    {
        return default;
    }

    public Task<UserEntity> Login(UserEntity user,string password)
    {   
        return default;
    }
}