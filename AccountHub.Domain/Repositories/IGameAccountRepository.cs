using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Repositories;

public interface IGameAccountRepository
{
    Task<GameAccountEntity?> GetAccountById(long id, CancellationToken cancellationToken =default);
    Task<IEnumerable<GameAccountEntity>> GetAccountsByUsername(string username, CancellationToken cancellationToken=default);
    Task<IEnumerable<GameAccountEntity>> GetAccountsByGame(string gameName, CancellationToken cancellationToken=default);
    Task<GameAccountEntity> AddGameAccount(GameAccountEntity gameEntity,CancellationToken cancellationToken=default);
    Task<int> DeleteGameAccount(long gameId,CancellationToken cancellationToken=default);

}