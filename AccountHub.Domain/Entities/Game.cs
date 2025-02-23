namespace AccountHub.Domain.Entities;

public class Game:BaseEntity
{
    public string Name { get; set; }
    public string Metadata { get; set; }

    public ICollection<GameAccount> Accounts { get; set; }
    public ICollection<GameService> Services { get; set; }
    public ICollection<GameVariant> Variants { get; set; }
}