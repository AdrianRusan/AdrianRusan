namespace PugPlatform.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid ActorId { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public Guid? TargetId { get; set; }
    public string? Payload { get; set; } // JSONB
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
