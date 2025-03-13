using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccountHub.Domain.Entities;

public class GameEntity:BaseEntity
{
    
    public required string Name { get; set; }
    public required JsonDocument Metadata { get; set; }
    [JsonIgnore] public ICollection<GameAccountEntity>? Accounts { get; set; }
    [JsonIgnore]
    public ICollection<GameServiceEntity>? Services { get; set; }
    [JsonIgnore]
    public ICollection<GameVariantEntity>? Variants { get; set; }
}