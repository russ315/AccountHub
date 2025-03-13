using Microsoft.AspNetCore.Http;

namespace AccountHub.Application.DTOs.Authentication;

public record UserRegisterDto(string Username, string Email,string Password,IFormFile? Image);
