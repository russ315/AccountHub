using AccountHub.Domain.Models;

namespace AccountHub.Domain.Entities;

public class NotificationEntity:BaseEntity
{

    public required string Message { get; set; }
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; }
    
    public required string UserId { get; set; }
    public UserEntity? User { get; set; }
}

