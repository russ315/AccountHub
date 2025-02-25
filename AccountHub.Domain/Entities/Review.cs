namespace AccountHub.Domain.Entities;

public class Review:BaseEntity
{
    public required string ReviewerId { get; set; }
    public UserEntity? Reviewer { get; set; }
    
    public required string RevieweeId  { get; set; }
    public  UserEntity? Reviewee  { get; set; }
}