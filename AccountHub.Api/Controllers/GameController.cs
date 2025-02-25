using AccountHub.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class GameController:ControllerBase
{
    private readonly IGameRepository _gameRepository;

    public GameController(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGameByIdAsync(long gameId)
    {
        var game =await  _gameRepository.GetById(gameId);
        return Ok(game);
    }
}