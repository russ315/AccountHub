using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IGameAccountRepository
{
    Task<GameAccountEntity?> GetAccountById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<GameAccountEntity>> GetAccountsByUsername(string username, CancellationToken cancellationToken);
    Task<IEnumerable<GameAccountEntity>> GetAccountsByGame(string gameName, CancellationToken cancellationToken);
    Task<GameAccountEntity> AddGameAccount(GameAccountEntity gameEntity);
    Task<GameAccountEntity> DeleteGameAccount(GameAccountEntity entity);
    Task SaveChangesAsync();
    Task<GameAccountEntity?> UpdateGameAccount(GameAccountEntity gameAccountEntity);
}