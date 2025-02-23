using AccountHub.Domain.Models;

namespace AccountHub.Domain.Entities;

public class GameAccount:BaseEntity
{
    public string Characteristics  { get; set; }
    public decimal Price { get; set; }
    public AccountStatus Status { get; set; }
   
    public Game Game { get; set; }
    public long GameId { get; set; }

    public UserEntity Seller { get; set; }
    public string SellerId { get; set; }

    public string? CurrentOwnerId { get; set; }
    public UserEntity CurrentOwner { get; set; }
    public ICollection<AccountImage> Images { get; set; }
}
