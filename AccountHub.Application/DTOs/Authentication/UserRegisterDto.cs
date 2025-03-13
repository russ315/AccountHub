using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace AccountHub.Application.DTOs;

public record UserRegisterDto(string Username, string Email,string Password,IFormFile? Image);
