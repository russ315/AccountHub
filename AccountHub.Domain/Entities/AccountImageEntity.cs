using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccountHub.Domain.Entities;

public class AccountImageEntity
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    
    public int Order { get; set; } 
    
    
    public long GameAccountId { get; set; }
    [JsonIgnore]
    public GameAccountEntity? GameAccount { get; set; }
}