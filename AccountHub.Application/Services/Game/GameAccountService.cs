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
using Microsoft.EntityFrameworkCore;

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

    public async Task DeleteGameAccountById(long id,CancellationToken cancellationToken)
    {
        var entity = await _gameAccountRepository.GetAccountById(id, cancellationToken);
        if(entity is null)
            throw new EntityNotFoundException("GameAccount is not found", $"GameAccount with id: {id} is not found");
        await _gameAccountRepository.DeleteGameAccount(entity);
        var images = await _accountImageRepository.GetImagesByGameAccount(id,cancellationToken);
        foreach (var image in entity.Images)
        {
            await _imageService.DeleteImage(image.ImageUrl);
            await _accountImageRepository.DeleteAccountImage(image.Id);
        }
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

    public async Task<GameAccountEntity> UpdateGameAccount(UpdateGameAccountDto model,CancellationToken cancellationToken)
    {
        var account = await _gameAccountRepository.GetAccountById(model.GameAccountId,cancellationToken);
        if (account == null)
            throw new EntityNotFoundException("Game Account", $"Game account with ID {model.GameAccountId} not found");

        // Update basic properties
        account.Update(
            model.Characteristics,
            model.Price,
            model.Status
        );

        // Update credentials if provided
        if (!string.IsNullOrEmpty(model.Credentials))
        {
            var credentials = JsonSerializer.Deserialize<List<GameCredential>>(model.Credentials);
            if (credentials == null)
                throw new BadRequestException("Invalid data", $"Invalid credentials: {model.Credentials}");

            // Remove existing credentials
            foreach (var type in account.GetAvailableCredentialTypes().ToList())
            {
                account.RemoveCredential(type);
            }

            // Add new credentials
            foreach (var credential in credentials)
            {
                account.AddCredential(credential.Type, credential.Value, credential.IsEncrypted, credential.ExpiresAt);
            }
        }

        // Handle image deletions
        if (model.ImagesToDelete != null && model.ImagesToDelete.Any())
        {
            foreach (var imageId in model.ImagesToDelete)
            {
                var image = await _accountImageRepository.GetImageById(imageId,cancellationToken);
                if (image != null)
                {
                    // Delete from storage
                    await _imageService.DeleteImage(image.ImageUrl);
                    // Delete from database
                    await _accountImageRepository.DeleteAccountImage(imageId);
                }
            }
        }

        // Handle new images
        if (model.NewImages != null && model.NewImages.Any())
        {
            var existingImages = await _accountImageRepository.GetImagesByGameAccount(model.GameAccountId,cancellationToken);
            var accountImageEntities = existingImages as AccountImageEntity[] ?? existingImages.ToArray();
            var nextOrder = accountImageEntities.Any() ? accountImageEntities.Max(x => x.Order) + 1 : 0;

            foreach (var image in model.NewImages)
            {
                var imageUrl = await _imageService.UploadImage(
                    Guid.NewGuid().ToString(),
                    image.OpenReadStream(),
                    CancellationToken.None
                );

                var accountImage = new AccountImageEntity
                {
                    GameAccountId = model.GameAccountId,
                    ImageUrl = imageUrl,
                    Order = nextOrder++
                };

                await _accountImageRepository.AddAccountImage(accountImage);
            }
        }

        // Save changes and handle concurrency
        var updatedAccount = await _gameAccountRepository.UpdateGameAccount(account);
        if (updatedAccount == null)
            throw new DbUpdateConcurrencyException("The game account has been modified by another user. Please refresh and try again.");

        // Dispatch domain events
        await _eventDispatcher.DispatchEventsAsync(account.DomainEvents);
        account.ClearDomainEvents();

        return updatedAccount;
    }
}