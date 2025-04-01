using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IGameAccountRepository
{
    Task<GameAccountEntity?> GetAccountById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<GameAccountEntity>> GetAccountsByUsername(string username, CancellationToken cancellationToken);
    Task<IEnumerable<GameAccountEntity>> GetAccountsByGame(string gameName, CancellationToken cancellationToken);
    Task<GameAccountEntity> AddGameAccount(GameAccountEntity gameEntity);
    Task<int> DeleteGameAccount(long gameId);
    Task SaveChangesAsync();
}