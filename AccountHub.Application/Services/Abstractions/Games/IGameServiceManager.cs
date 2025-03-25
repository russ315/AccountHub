using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Abstractions.Games;

public interface IGameServiceManager
{
    Task<GameServiceEntity> GetGameServiceById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<GameServiceEntity>> GetGameServicesByUsername(string username, CancellationToken cancellationToken);
    Task<IEnumerable<GameServiceEntity>> GetGameServicesByGame(string gameName, CancellationToken cancellationToken);
    Task<GameServiceEntity> CreateGameService(CreateGameServiceDto model);

    Task DeleteGameServiceById(long id);
}

