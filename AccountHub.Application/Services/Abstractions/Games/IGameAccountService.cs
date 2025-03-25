using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Abstractions.Games;

public interface IGameAccountService
{
    Task<GameAccountEntity> GetGameAccountById(long id,CancellationToken cancellationToken);
    Task<IEnumerable<GameAccountEntity>> GetGameAccountsByUsername(string username,CancellationToken cancellationToken);
    Task<IEnumerable<GameAccountEntity>> GetGameAccountsByGame(string gameName,CancellationToken cancellationToken);
    Task<GameAccountEntity> CreateGameAccount(CreateGameAccountDto model,CancellationToken cancellationToken);

    Task DeleteGameAccountById(long id);
}