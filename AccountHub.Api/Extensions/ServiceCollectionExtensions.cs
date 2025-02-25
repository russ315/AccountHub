using AccountHub.Application.Services;
using AccountHub.Application.Services.Game;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Interfaces;
using AccountHub.Infrastructure.Data;
using AccountHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountHub.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        return builder;
        
    }
    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options=>options.UseNpgsql
            (builder.Configuration.GetConnectionString("DefaultConnection")));
        return builder;
        
    }
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        return builder;
        
    }
    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        return builder;
        
    }
    public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
    {
        return builder;

    }
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IGameAccountRepository, GameAccountRepository>();
        builder.Services.AddScoped<IGameServiceRepository, GameServiceRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IGameAccountService, GameAccountService>();
        builder.Services.AddScoped<IGameServiceManager, GameServiceManager>();


        return builder;
    }

    

}