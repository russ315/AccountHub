using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Game;

public interface IGameService
{
    Task<GameEntity> GetGameById(long id);
    Task<GameEntity> AddGame(CreateGameDto model);
    Task DeleteGameById(long id);

}