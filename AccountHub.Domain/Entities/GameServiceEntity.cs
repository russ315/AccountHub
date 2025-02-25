using System.Text.Json;
using AccountHub.Domain.Models;

namespace AccountHub.Domain.Entities;

public class GameServiceEntity:BaseEntity
{
    public ServiceType Type { get; set; }
    public required JsonDocument ServiceMetadata { get; set; }

    public long GameId { get; set; }
    public  GameEntity? Game { get; set; }

    public required string ProviderId { get; set; }
    public  UserEntity? Provider { get; set; }
    public ICollection<ServiceScheduleEntity> Schedules { get; set; }
}