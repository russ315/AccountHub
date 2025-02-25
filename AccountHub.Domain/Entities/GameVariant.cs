namespace AccountHub.Domain.Entities;

public class GameVariant:BaseEntity
{
    public required string Name { get; set; }
    public required string DataType { get; set; }
    public required string ValidationRules { get; set; }

    public long GameId { get; set; }
    public Game? Game { get; set; }
}