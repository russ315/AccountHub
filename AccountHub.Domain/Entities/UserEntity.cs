using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace AccountHub.Domain.Entities;

public class UserEntity:IdentityUser
{
    public string? ImageUrl { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public  ICollection<GameAccountEntity>? GameAccounts { get; set; }
    public  ICollection<TransactionEntity>? Transactions { get; set; }
    public  ICollection<ReviewEntity>? ReviewsGiven { get; set; }
    public  ICollection<ReviewEntity>? ReviewsReceived { get; set; }
    public  ICollection<GameServiceEntity>? ServicesProvided { get; set; }
    public  ICollection<NotificationEntity>? Notifications { get; set; }
}