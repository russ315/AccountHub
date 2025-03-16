using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace AccountHub.Application.DTOs.Game;


public record CreateGameAccountDto(long GameId,string Characteristics,decimal Price,int Status,string SellerId,string CurrentOwnerId,List<IFormFile> Images);