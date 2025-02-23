using AccountHub.Domain.Models;

namespace AccountHub.Domain.Entities;

public class Notification:BaseEntity
{

    public string Message { get; set; }
    public bool IsRead { get; set; }
    public NotificationType Type { get; set; }
    
    public string UserId { get; set; }
    public UserEntity User { get; set; }
}

