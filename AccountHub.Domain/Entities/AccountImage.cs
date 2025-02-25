namespace AccountHub.Domain.Entities;

public class AccountImage
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsMain { get; set; } 
    public int Order { get; set; } 
    
    
    public long GameAccountId { get; set; }
    public GameAccount? GameAccount { get; set; }
}