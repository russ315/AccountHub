namespace AccountHub.Domain.Entities;

public class GameVariant:BaseEntity
{
    public string Name { get; set; }
    public string DataType { get; set; }
    public string ValidationRules { get; set; }

    public long GameId { get; set; }
    public Game  Game { get; set; }
}