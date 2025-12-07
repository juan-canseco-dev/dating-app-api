namespace DatingApp.Domain.Users;

public class UserBlock
{
    
    private UserBlock() {}

    public UserBlock(string blockedUserId, DateTime blockedAt)
    {
        BlockedUserId = blockedUserId;
        BlockedAt = blockedAt;
    }

    public string Id { get; } = default!;
    public string? BlockedUserId { get; }
    public virtual User? BlockedUser { get; }
    public DateTime BlockedAt { get; }
}
