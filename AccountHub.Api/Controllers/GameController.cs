using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Services.Abstractions.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
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
        var game =await  _gameService.GetGameById(gameId, HttpContext.RequestAborted);

        return Ok(game);
    }
    [HttpDelete("{id}")]
    [Authorize("Admin")]
    public async Task<IActionResult> DeleteGameById(long id)
    {
        await _gameService.DeleteGameById(id);
        return Ok();
    }
    [HttpPost]
    [Authorize("Admin")]
    public async Task<IActionResult> AddGame(CreateGameDto model)
    {
        
        var game =await  _gameService.AddGame(model);
        return Ok(game);
    }
}