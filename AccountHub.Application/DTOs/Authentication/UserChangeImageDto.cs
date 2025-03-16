using Microsoft.AspNetCore.Http;

namespace AccountHub.Application.DTOs.Authentication;

public record UserChangeImageDto(string UserName, IFormFile Image);