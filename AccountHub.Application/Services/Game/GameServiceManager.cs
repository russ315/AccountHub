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
    
    public async Task<GameServiceEntity> GetGameServiceById(long id, CancellationToken cancellationToken)
    {
        var game = await _gameServiceRepository.GetAccountServiceById(id, cancellationToken);
        if(game is null)
            throw new EntityNotFoundException("Game not found",$"Game with id: {id} is not found");
        return game;
    }

    public async Task<IEnumerable<GameServiceEntity>> GetGameServicesByUsername(string username,
        CancellationToken cancellationToken)
    {
        var services = await _gameServiceRepository.GetAccountServicesByUsername(username, cancellationToken);
        return services;    }

    public async Task<IEnumerable<GameServiceEntity>> GetGameServicesByGame(string gameName,
        CancellationToken cancellationToken)
    {
        var services = await _gameServiceRepository.GetAccountServicesByGame(gameName, cancellationToken);
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
            throw new EntityNotFoundException("Game is not found",$"GameAccount with id: {id} is not found");

    }
}