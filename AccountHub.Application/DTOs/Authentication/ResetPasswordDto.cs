namespace AccountHub.Application.DTOs.Authentication;

public record ResetPasswordDto(string Email, string NewPassword,string Token);