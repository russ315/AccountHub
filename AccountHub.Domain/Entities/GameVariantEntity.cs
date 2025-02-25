using System.Text.Json;

namespace AccountHub.Domain.Entities;

public class GameVariantEntity:BaseEntity
{
    public required string Name { get; set; }
    public required string DataType { get; set; }
    public required JsonDocument ValidationRules { get; set; }

    public long GameId { get; set; }
    public GameEntity? Game { get; set; }
}