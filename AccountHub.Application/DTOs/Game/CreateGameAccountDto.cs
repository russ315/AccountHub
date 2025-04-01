using System.Text.Json;
using AccountHub.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace AccountHub.Application.DTOs.Game;


public record CreateGameAccountDto(long GameId,string Characteristics,string Credentials,decimal Price,int Status,string SellerId,string CurrentOwnerId,IFormFileCollection Images);