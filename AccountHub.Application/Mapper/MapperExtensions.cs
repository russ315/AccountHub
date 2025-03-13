
using AccountHub.Application.DTOs.Authentication;
using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Models;

namespace AccountHub.Application.Mapper;

public static class MapperExtensions
{
    public static GameEntity ToEntity(this CreateGameDto model)
    {
        return new GameEntity()
        {
            Name = model.Name,
            Metadata = model.Metadata
        };
    }
    public static GameAccountEntity ToEntity(this CreateGameAccountDto model)
    {
        return new GameAccountEntity()
        {
            Price = model.Price,
            Characteristics= model.Characteristics,
            SellerId = model.SellerId,
            CurrentOwnerId = model.CurrentOwnerId,
            Status = (AccountStatus)model.Status,
            GameId = model.GameId,
        };
    }
    
    public static GameServiceEntity ToEntity(this CreateGameServiceDto model)
    {
        return new GameServiceEntity()
        {
            Type = (ServiceType)model.Type,
            ServiceMetadata = model.MetaData,
            ProviderId = model.Providerid,
            GameId = model.GameId,
        };
    }

    public static UserEntity ToEntity(this UserRegisterDto model)
    {
        return new UserEntity()
        {
            UserName = model.Username,
            Email = model.Email,

        };
    }
    
    
}