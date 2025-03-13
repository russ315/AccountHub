using System.Text.Json;
using AccountHub.Domain.Models;

namespace AccountHub.Domain.Entities;

public  class GameAccountEntity:BaseEntity
{
    public required JsonDocument Characteristics  { get; set; }
    public decimal Price { get; set; }
    public AccountStatus Status { get; set; }
   
    public GameEntity? Game { get; set; }
    public long GameId { get; set; }

    public  UserEntity? Seller { get; set; }
    public required string SellerId { get; set; }

    public required string CurrentOwnerId { get; set; }
    public UserEntity? CurrentOwner { get; set; }
    public ICollection<AccountImageEntity> Images { get; set; } = null!;
}
