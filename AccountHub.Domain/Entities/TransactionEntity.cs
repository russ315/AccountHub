namespace AccountHub.Domain.Entities;

public class TransactionEntity:BaseEntity
{
    public decimal Amount { get; set; }
    
    public required string BuyerId { get; set; }
    public UserEntity? Buyer { get; set; }

    public long GameAccountId { get; set; }
    public GameAccountEntity? GameAccount { get; set; }
}