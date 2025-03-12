﻿using System.Text;
using AccountHub.Api.Handlers;
using AccountHub.Application.Services;
using AccountHub.Application.Services.Abstractions;
using AccountHub.Application.Services.Abstractions.Games;
using AccountHub.Application.Services.Game;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Models;
using AccountHub.Domain.Options;
using AccountHub.Domain.Repositories;
using AccountHub.Domain.Services;
using AccountHub.Infrastructure.Data;
using AccountHub.Infrastructure.Repositories;
using AccountHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AccountHub.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "AccountHub API", Version = "v1" });
            options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "X-DeviceId",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme.",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {{
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return builder;
        
    }
    public static WebApplicationBuilder AddData(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options=>options.UseNpgsql
            (builder.Configuration.GetConnectionString("DefaultConnection")));
        return builder;
        
    }
    
    public static WebApplicationBuilder AddExceptionHandler(this WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        return builder;
        
    }
    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:SecretKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                    ValidAudience = builder.Configuration["JwtOptions:Audience"],
                    LifetimeValidator = (before, expires, token, parameters) => expires > DateTime.UtcNow

                };
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived =  (context) =>  Task.FromResult(context.Token = context.Request.Query["access_token"]) 

                };
            });
        builder.Services
            .AddDefaultIdentity<UserEntity>()
            .AddRoles<IdentityRole>()
            .AddUserManager<UserManager<UserEntity>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<UserEntity>>()
            .AddEntityFrameworkStores<DataContext>();
        
        
        
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", o => o.RequireRole(RoleConsts.Admin));
            options.AddPolicy("Merchant", o => o.RequireRole(RoleConsts.Merchant));
            options.AddPolicy("User", o => o.RequireRole(RoleConsts.User));

        });

        return builder;
        
    }

    public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
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
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IGameAccountService, GameAccountService>();
        builder.Services.AddScoped<IGameServiceManager, GameServiceManager>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IJwtService, JwtService>();

        return builder;
    }

    

}