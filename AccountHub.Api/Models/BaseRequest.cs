using System.ComponentModel.DataAnnotations;

namespace AccountHub.Api.Models;

public abstract class BaseRequest
{
    [Required]
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
} 