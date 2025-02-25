using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Game;

public interface IGameServiceManager
{
    Task<GameServiceEntity> GetGameServiceById(long id);
    Task<IEnumerable<GameServiceEntity>> GetGameServicesByUsername(string username);
    Task<IEnumerable<GameServiceEntity>> GetGameServicesByGame(string gameName);
    Task<GameServiceEntity> CreateGameService(CreateGameServiceDto model);
    Task DeleteGameServiceById(long id);

}

