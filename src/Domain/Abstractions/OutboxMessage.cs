namespace DatingApp.Domain.Abstractions;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationTime { get; set; }
    public DateTime? CompleteTime { get; set; }
    public string? Exception { get; set; }
}