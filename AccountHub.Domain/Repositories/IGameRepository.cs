using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IGameRepository
{
    Task<GameEntity?> GetById(long id,CancellationToken cancellationToken);
    Task<GameEntity?> GetByName(string name,CancellationToken cancellationToke);

    Task<GameEntity> AddGame(GameEntity gameEntity);
    Task<int> DeleteGame(long gameId);

}