using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Mapper;
using AccountHub.Application.Services.Game;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AccountHub.Application.Services;

public class GameAccountService:IGameAccountService
{
    private readonly IGameAccountRepository _gameAccountRepository;

    public GameAccountService(IGameAccountRepository gameAccountRepository)
    {
        _gameAccountRepository = gameAccountRepository;
    }
    public async Task<GameAccountEntity> GetGameAccountById(long id)
    {
        var game = await _gameAccountRepository.GetAccountById(id);
        if(game is null)
            throw new EntityNotFoundException($"GameAccount with id: {id} not found");
        return game;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetGameAccountsByUsername(string username)
    {
        var games = await _gameAccountRepository.GetAccountsByUsername(username);
        return games;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetGameAccountsByGame(string gameName)
    {
        var games = await _gameAccountRepository.GetAccountsByGame(gameName);
        return games;
        
    }

    public Task<GameAccountEntity> CreateGameAccount(CreateGameAccountDto model)
    {
        var entity = model.ToEntity();
        
        var createGameAccountResult = _gameAccountRepository.AddGameAccount(entity);
        
        return createGameAccountResult;
    }

    public async Task DeleteGameAccountById(long id)
    {
        var totalRowsDeleted =await  _gameAccountRepository.DeleteGameAccount(id);
        if(totalRowsDeleted ==0)
            throw new EntityNotFoundException($"GameAccount with id: {id} not found");
        
    }
}