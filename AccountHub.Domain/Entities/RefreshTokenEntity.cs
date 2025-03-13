using System.ComponentModel.DataAnnotations;

namespace AccountHub.Domain.Entities;

public class RefreshTokenEntity:BaseEntity
{
    [Required]
    public required string DeviceId { get; set; }
    [Required]
    public required string Token { get; set; } 
    [Required]
    public DateTime Expires { get; set; }
    public UserEntity? User { get; set; }
    public required string UserId { get; set; }
    
}