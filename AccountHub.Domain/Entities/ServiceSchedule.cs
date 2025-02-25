namespace AccountHub.Domain.Entities;

public class ServiceSchedule:BaseEntity
{
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
    
    public bool IsBooked { get; set; }
    public bool IsConfirmed { get; set; }
    
    public long GameServiceId { get; set; }
    public GameService? GameService { get; set; }
    
    public string? ClientId { get; set; }
    public UserEntity? Client { get; set; }
}