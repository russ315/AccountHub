using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Interfaces;

public interface IGameRepository
{
    Task<GameEntity> GetById(long id,CancellationToken cancellationToken=default);
    Task<GameEntity?> GetByName(string name,CancellationToken cancellationToken=default);

    Task<GameEntity> AddGame(GameEntity gameEntity,CancellationToken cancellationToken=default);
    Task<int> DeleteGame(long gameId,CancellationToken cancellationToken=default);

}