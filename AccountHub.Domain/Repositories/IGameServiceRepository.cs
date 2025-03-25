using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IGameServiceRepository
{
    Task<GameServiceEntity?> GetAccountServiceById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<GameServiceEntity>> GetAccountServicesByUsername(string username, CancellationToken cancellationToken);
    Task<IEnumerable<GameServiceEntity>> GetAccountServicesByGame(string gameName, CancellationToken cancellationToken);
    Task<GameServiceEntity> AddGameService(GameServiceEntity entity);
    Task<int> DeleteGameService(long id);

}