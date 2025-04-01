using System.Text.Json;
using AccountHub.Domain.Models;
using AccountHub.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace AccountHub.Application.DTOs.Game;

public record UpdateGameAccountDto(
    long GameAccountId,
    JsonDocument Characteristics,
    decimal Price,
    AccountStatus Status,
    string? Credentials,
    List<IFormFile>? NewImages,
    List<int>? ImagesToDelete
);