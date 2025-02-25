using AccountHub.Application.DTOs.Game;
using AccountHub.Application.Services.Game;
using Microsoft.AspNetCore.Mvc;

namespace AccountHub.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class GameServiceController:Controller
{
    private readonly IGameServiceManager _gameServiceManager;

    public GameServiceController(IGameServiceManager gameServiceManager)
    {
        _gameServiceManager = gameServiceManager;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGameService(long id)
    {
        var gameService = await _gameServiceManager.GetGameServiceById(id);
        return Ok(gameService);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGameServiceById(long id)
    {
        await _gameServiceManager.DeleteGameServiceById(id);
        return Ok();
    }
    
    [HttpGet("get-by-username/{username}")]
    public async Task<IActionResult> GetGameServicesOfUser(string username)
    {
        var gameServices = await _gameServiceManager.GetGameServicesByUsername(username);
        
        return Ok(gameServices);
    }
    [HttpGet("get-by-game/{game}")]
    public async Task<IActionResult> GetGameServicesOfGame(string game)
    {
        var gameServices = await _gameServiceManager.GetGameServicesByGame(game);
        
        return Ok(gameServices);
    }
    [HttpPost()]
    public async Task<IActionResult> AddGameService(CreateGameServiceDto model)
    {
        var game =await  _gameServiceManager.CreateGameService(model);
        return Ok(game);


    }
}