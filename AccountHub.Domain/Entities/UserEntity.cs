using Microsoft.AspNetCore.Identity;

namespace AccountHub.Domain.Entities;

public class UserEntity:IdentityUser
{
    public string? ImageUrl { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GameAccount>? GameAccounts { get; set; }
    public ICollection<Transaction>? Transactions { get; set; }
    public ICollection<Review>? ReviewsGiven { get; set; }
    public ICollection<Review>? ReviewsReceived { get; set; }
    public ICollection<GameService>? ServicesProvided { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}