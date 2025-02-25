using AccountHub.Application.DTOs.Game;
using AccountHub.Domain.Entities;

namespace AccountHub.Application.Services.Game;

public interface IGameAccountService
{
    Task<GameAccountEntity> GetGameAccountById(long id);
    Task<IEnumerable<GameAccountEntity>> GetGameAccountsByUsername(string username);
    Task<IEnumerable<GameAccountEntity>> GetGameAccountsByGame(string gameName);
    Task<GameAccountEntity> CreateGameAccount(CreateGameAccountDto model);
    Task DeleteGameAccountById(long id);

}