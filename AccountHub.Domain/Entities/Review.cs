namespace AccountHub.Domain.Entities;

public class Review:BaseEntity
{
    public string ReviewerId { get; set; }
    public UserEntity Reviewer { get; set; }
    
    public string RevieweeId  { get; set; }
    public UserEntity Reviewee  { get; set; }
}