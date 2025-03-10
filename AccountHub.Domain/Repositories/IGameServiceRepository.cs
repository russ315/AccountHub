using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IGameServiceRepository
{
    Task<GameServiceEntity?> GetAccountServiceById(long id, CancellationToken cancellationToken =default);
    Task<IEnumerable<GameServiceEntity>> GetAccountServicesByUsername(string username, CancellationToken cancellationToken=default);
    Task<IEnumerable<GameServiceEntity>> GetAccountServicesByGame(string gameName, CancellationToken cancellationToken=default);
    Task<GameServiceEntity> AddGameService(GameServiceEntity entity,CancellationToken cancellationToken=default);
    Task<int> DeleteGameService(long id,CancellationToken cancellationToken=default);

}