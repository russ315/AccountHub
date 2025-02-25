using System.Text.Json;

namespace AccountHub.Application.DTOs.Game;


//Implement images
public record CreateGameAccountDto(long GameId,JsonDocument Characteristics,decimal Price,int Status,string SellerId,string CurrentOwnerId);