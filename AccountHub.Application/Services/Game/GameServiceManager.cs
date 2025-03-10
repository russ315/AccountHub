using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Mapper;
using AccountHub.Application.Services.Abstractions.Games;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Repositories;

namespace AccountHub.Application.Services.Game;

public class GameServiceManager:IGameServiceManager
{
    private readonly IGameServiceRepository _gameServiceRepository;

    public GameServiceManager(IGameServiceRepository gameServiceRepository)
    {
        _gameServiceRepository = gameServiceRepository;
    }
    
    public async Task<GameServiceEntity> GetGameServiceById(long id)
    {
        var game = await _gameServiceRepository.GetAccountServiceById(id);
        if(game is null)
            throw new EntityNotFoundException($"Game with id: {id} not found");
        return game;
    }

    public async Task<IEnumerable<GameServiceEntity>> GetGameServicesByUsername(string username)
    {
        var services = await _gameServiceRepository.GetAccountServicesByUsername(username);
        return services;    }

    public async Task<IEnumerable<GameServiceEntity>> GetGameServicesByGame(string gameName)
    {
        var services = await _gameServiceRepository.GetAccountServicesByGame(gameName);
        return services;    }

    public async Task<GameServiceEntity> CreateGameService(CreateGameServiceDto model)
    {
        var gameServiceEntity = model.ToEntity();
        var gameServiceResult =await _gameServiceRepository.AddGameService(gameServiceEntity);
        return gameServiceResult;
    }

    public async Task DeleteGameServiceById(long id)
    {
        var totalRowsDeleted =await  _gameServiceRepository.DeleteGameService(id);
        if(totalRowsDeleted ==0)
            throw new EntityNotFoundException($"GameAccount with id: {id} not found");

    }
}