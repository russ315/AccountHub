using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Services.Abstractions.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameAccountController:Controller
{
    private readonly IGameAccountService _gameAccountService;

    public GameAccountController(IGameAccountService gameAccountService)
    {
        _gameAccountService = gameAccountService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGameAccount(long id)
    {
        var gameAccount = await _gameAccountService.GetGameAccountById(id);
        return Ok(gameAccount);
    }
    
    [HttpGet("get-by-username/{username}")]
    public async Task<IActionResult> GetGameAccountsOfUser(string username)
    {
        var gameAccounts = await _gameAccountService.GetGameAccountsByUsername(username);
        
        return Ok(gameAccounts);
    }
    [HttpGet("get-by-game/{game}")]
    public async Task<IActionResult> GetGameAccountsOfGame(string game)
    {
        var gameAccounts = await _gameAccountService.GetGameAccountsByGame(game);
        
        return Ok(gameAccounts);
    }
    [HttpDelete("{id}")]
    [Authorize("Merchant")]
    public async Task<IActionResult> DeleteGameAccountById(long id)
    {
        await _gameAccountService.DeleteGameAccountById(id);
        return Ok();
    }
    [HttpPost]
    [Authorize("Merchant")]
    public async Task<IActionResult> AddGameAccount([FromForm] CreateGameAccountDto model)
    {
        var game =await  _gameAccountService.CreateGameAccount(model);
        return Ok(game);


    }
}