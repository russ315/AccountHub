using System.Text.Json;

namespace AccountHub.Application.DTOs.Game;

public record CreateGameServiceDto(int Type,JsonDocument MetaData,long GameId,string Providerid);