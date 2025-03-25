using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Mapper;
using AccountHub.Application.Services.Abstractions.Games;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Repositories;
using AccountHub.Domain.Services;

namespace AccountHub.Application.Services.Game;

public class GameAccountService:IGameAccountService
{
    private readonly IGameAccountRepository _gameAccountRepository;
    private readonly IAccountImageRepository _accountImageRepository;
    private readonly IImageService _imageService;

    public GameAccountService(IGameAccountRepository gameAccountRepository,IAccountImageRepository accountImageRepository,IImageService imageService)
    {
        _gameAccountRepository = gameAccountRepository;
        _accountImageRepository = accountImageRepository;
        _imageService = imageService;
    }
    public async Task<GameAccountEntity> GetGameAccountById(long id,CancellationToken cancellationToken)
    {
        var game = await _gameAccountRepository.GetAccountById(id,cancellationToken);
        if(game is null)
            throw new EntityNotFoundException("GameAccount is not found",$"GameAccount with id: {id} is not found");
        return game;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetGameAccountsByUsername(string username,CancellationToken cancellationToken)
    {
        var games = await _gameAccountRepository.GetAccountsByUsername(username,cancellationToken);
        return games;
    }

    public async Task<IEnumerable<GameAccountEntity>> GetGameAccountsByGame(string gameName,CancellationToken cancellationToken)
    {
        var games = await _gameAccountRepository.GetAccountsByGame(gameName,cancellationToken);
        return games;
        
    }

    public async Task<GameAccountEntity> CreateGameAccount(CreateGameAccountDto model,CancellationToken cancellationToken)
    {
        var entity = model.ToEntity();
        
        var createGameAccountResult = await _gameAccountRepository.AddGameAccount(entity);
        for (int i = 0; i < model.Images.Count; i++)
        {
            var imageUrl = await _imageService
                .UploadImage(Guid.NewGuid().ToString(), model.Images[i].OpenReadStream(),cancellationToken);
            var accountImage = new AccountImageEntity
            {
                GameAccountId = createGameAccountResult.Id,
                ImageUrl = imageUrl,
                Order = i
            };
            await _accountImageRepository.AddAccountImage(accountImage);
        }
        return createGameAccountResult;
    }

    public async Task DeleteGameAccountById(long id)
    {
        var totalRowsDeleted =await  _gameAccountRepository.DeleteGameAccount(id);
        if(totalRowsDeleted ==0)
            throw new EntityNotFoundException("GameAccount is not found",$"GameAccount with id: {id} is not found");
        
    }
}