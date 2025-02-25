using System.Net.Http.Json;
using System.Text.Json;

namespace AccountHub.Application.DTOs.Game;

public record CreateGameDto(string Name,JsonDocument Metadata);