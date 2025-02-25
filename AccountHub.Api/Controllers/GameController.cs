using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Services.Game;
using AccountHub.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class GameController:Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGame(long gameId)
    {
        var game =await  _gameService.GetGameById(gameId);

        return Ok(game);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGameById(long id)
    {
        await _gameService.DeleteGameById(id);
        return Ok();
    }
    [HttpPost()]
    public async Task<IActionResult> AddGame(CreateGameDto model)
    {
        
        var game =await  _gameService.AddGame(model);
        return Ok(game);
    }
}