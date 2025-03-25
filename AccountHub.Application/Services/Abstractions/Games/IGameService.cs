using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Abstractions.Games;

public interface IGameService
{
    Task<GameEntity> GetGameById(long id, CancellationToken cancellationToken);
    Task<GameEntity> AddGame(CreateGameDto model);

    Task DeleteGameById(long id);
}