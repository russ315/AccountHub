using AccountHub.Domain.Models;

namespace AccountHub.Domain.Entities;

public class GameService:BaseEntity
{
    public ServiceType Type { get; set; }
    public string ServiceMetadata { get; set; }

    public long GameId { get; set; }
    public Game Game { get; set; }

    public string ProviderId { get; set; }
    public UserEntity Provider { get; set; }
    public ICollection<ServiceSchedule> Schedules { get; set; }
}