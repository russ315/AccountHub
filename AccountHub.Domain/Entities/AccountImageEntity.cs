namespace AccountHub.Domain.Entities;

public class AccountImageEntity
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    public bool IsMain { get; set; } 
    public int Order { get; set; } 
    
    
    public long GameAccountId { get; set; }
    public GameAccountEntity? GameAccount { get; set; }
}