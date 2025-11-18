namespace PugPlatform.Domain.Entities;

public class ApplicationComment
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid AuthorId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public bool IsInternal { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Application Application { get; set; } = null!;
    public User Author { get; set; } = null!;
}
