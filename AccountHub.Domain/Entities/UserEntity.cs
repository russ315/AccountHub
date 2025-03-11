using System.ComponentModel.DataAnnotations.Schema;
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
    [NotMapped]

    public  ICollection<GameServiceEntity>? ServicesProvided { get; set; }
    [NotMapped]
    public  ICollection<NotificationEntity>? Notifications { get; set; }
    
    [NotMapped]
    public  ICollection<RefreshTokenEntity>? RefreshTokens { get; set; }

}