
using System.Text.Json;
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
        return new GameAccountEntity(
            JsonDocument.Parse(model.Characteristics), model.Price,
            (AccountStatus)model.Status, model.GameId, model.SellerId, model.CurrentOwnerId);

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