using AccountHub.Domain.Entities;

namespace AccountHub.Domain.Interfaces;

public interface IGameAccountRepository
{
    Task<GameAccount?> GetAccountById(long id, CancellationToken cancellationToken =default);
    Task<IEnumerable<GameAccount>> GetAccountsByUsername(string username, CancellationToken cancellationToken=default);
    Task<IEnumerable<GameAccount>> GetAccountsByGame(string gameName, CancellationToken cancellationToken=default);

}