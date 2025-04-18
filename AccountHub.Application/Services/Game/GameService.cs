﻿using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Mapper;
using AccountHub.Application.Services.Abstractions.Games;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Repositories;

namespace AccountHub.Application.Services.Game;

public class GameService:IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    public async Task<GameEntity> GetGameById(long id, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetById(id,cancellationToken);
        if (game is null)
            throw new EntityNotFoundException("Game is not found",$"Game with id: {id} is not found");
           
        return game;
    }

    public async Task<GameEntity> AddGame(CreateGameDto model)
    {
        var game = model.ToEntity();
        var gameCreateResult = await _gameRepository.AddGame(game);
        return gameCreateResult;
    }

    public async Task DeleteGameById(long id)
    {
        var totalRowsDeleted =await  _gameRepository.DeleteGame(id);
        if(totalRowsDeleted ==0)
            throw new EntityNotFoundException("GameAccount is not found",$"GameAccount with id: {id} is not found");

    }
}