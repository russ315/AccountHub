using System.Text.Json;
using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Events;
using AccountHub.Application.Mapper;
using AccountHub.Application.Services.Abstractions.Games;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Models;
using AccountHub.Domain.Repositories;
using AccountHub.Domain.Services;
using AccountHub.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AccountHub.Application.Services.Game;

public class GameAccountService : IGameAccountService
{
    private readonly IGameAccountRepository _gameAccountRepository;
    private readonly IAccountImageRepository _accountImageRepository;
    private readonly IImageService _imageService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IDomainEventDispatcher _eventDispatcher;
    private readonly ILogger<GameAccountService> _logger;

    public GameAccountService(
        IGameAccountRepository gameAccountRepository,
        IAccountImageRepository accountImageRepository,
        IImageService imageService,
        UserManager<UserEntity> userManager,
        IDomainEventDispatcher eventDispatcher,
        ILogger<GameAccountService> logger)
    {
        _gameAccountRepository = gameAccountRepository;
        _accountImageRepository = accountImageRepository;
        _imageService = imageService;
        _userManager = userManager;
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public async Task<GameAccountEntity> GetGameAccountById(long id, CancellationToken cancellationToken)
    {
        var game = await _gameAccountRepository.GetAccountById(id, cancellationToken);
        if (game is null)
            throw new EntityNotFoundException("GameAccount is not found", $"GameAccount with id: {id} is not found");
        return game;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetGameAccountsByUsername(string username, CancellationToken cancellationToken)
    {
        var games = await _gameAccountRepository.GetAccountsByUsername(username, cancellationToken);
        return games;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetGameAccountsByGame(string gameName, CancellationToken cancellationToken)
    {
        var games = await _gameAccountRepository.GetAccountsByGame(gameName, cancellationToken);
        return games;
    }

    public async Task<GameAccountEntity> CreateGameAccount(CreateGameAccountDto model, CancellationToken cancellationToken)
    {
        var entity = model.ToEntity();
        var credentials = JsonSerializer.Deserialize<List<GameCredential>>(model.Credentials);
        if(credentials is null)
            throw new BadRequestException("Invalid data",$"Invalid credentials:{model.Credentials}");
            
        foreach(var credential in credentials)
            entity.AddCredential(credential.Type,credential.Value,credential.IsEncrypted,credential.ExpiresAt);
        var createGameAccountResult = await _gameAccountRepository.AddGameAccount(entity);
        for (int i = 0; i < model.Images.Count; i++)
        {
            var imageUrl = await _imageService
                .UploadImage(Guid.NewGuid().ToString(), model.Images[i].OpenReadStream(), cancellationToken);
            var accountImage = new AccountImageEntity
            {
                GameAccountId = createGameAccountResult.Id,
                ImageUrl = imageUrl,
                Order = i
            };
            await _accountImageRepository.AddAccountImage(accountImage);
        }
        await _eventDispatcher.DispatchEventsAsync(createGameAccountResult.DomainEvents);
        createGameAccountResult.ClearDomainEvents();
        
        return createGameAccountResult;
    }

    public async Task DeleteGameAccountById(long id)
    {
        var totalRowsDeleted = await _gameAccountRepository.DeleteGameAccount(id);
        if(totalRowsDeleted == 0)
            throw new EntityNotFoundException("GameAccount is not found", $"GameAccount with id: {id} is not found");
    }

    public async Task<GameAccountEntity> SellGameAccountAsync(long gameAccountId, string buyerId)
    {
        // Check if game account exists
        var gameAccount = await _gameAccountRepository.GetAccountById(gameAccountId, CancellationToken.None);
        if (gameAccount == null)
            throw new EntityNotFoundException("Game account not found", $"Game account with id: {gameAccountId} is not found");

        // Check if buyer exists
        var buyer = await _userManager.FindByIdAsync(buyerId);
        if (buyer == null)
            throw new ArgumentException("Buyer not found", nameof(buyerId));

        // Transfer ownership
        gameAccount.TransferOwnership(buyerId);
        gameAccount.UpdateStatus(AccountStatus.Sold);
        
        // Save changes
        await _gameAccountRepository.SaveChangesAsync();

        // Dispatch domain events
        await _eventDispatcher.DispatchEventsAsync(gameAccount.DomainEvents);
        gameAccount.ClearDomainEvents();

        return gameAccount;
    }

    public async Task<GameAccountEntity> UpdateGameAccountStatusAsync(long gameAccountId, AccountStatus status)
    {
        // Check if game account exists
        var gameAccount = await _gameAccountRepository.GetAccountById(gameAccountId, CancellationToken.None);
        if (gameAccount == null)
            throw new EntityNotFoundException("Game account not found", $"Game account with id: {gameAccountId} is not found");

        // Update status
        gameAccount.UpdateStatus(status);

        // Save changes
        await _gameAccountRepository.SaveChangesAsync();

        // Dispatch domain events
        await _eventDispatcher.DispatchEventsAsync(gameAccount.DomainEvents);
        gameAccount.ClearDomainEvents();

        return gameAccount;
    }
}