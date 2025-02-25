using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Domain.Entities;

public class UserEntity:IdentityUser
{
    public string? ImageUrl { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public virtual ICollection<GameAccountEntity>? GameAccounts { get; set; }
    public virtual ICollection<TransactionEntity>? Transactions { get; set; }
    public virtual ICollection<ReviewEntity>? ReviewsGiven { get; set; }
    public virtual ICollection<ReviewEntity>? ReviewsReceived { get; set; }
    public virtual ICollection<GameServiceEntity>? ServicesProvided { get; set; }
    public virtual ICollection<NotificationEntity>? Notifications { get; set; }
}