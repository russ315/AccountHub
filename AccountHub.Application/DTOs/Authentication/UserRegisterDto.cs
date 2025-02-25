namespace AccountHub.Application.DTOs;

public record UserRegisterDto(string Username, string Email,string Password,byte[] Image);
