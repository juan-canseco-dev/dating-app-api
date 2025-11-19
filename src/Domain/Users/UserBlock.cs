
using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users;

public class UserBlock : Entity<string>
{
    
    private UserBlock() {}

    public UserBlock(string blockedUserId, DateTime blockedAt)
    {
        BlockedUserId = blockedUserId;
        BlockedAt = blockedAt;
    }

    public string? BlockedUserId { get; }
    public virtual User? BlockedUser { get; }
    public DateTime BlockedAt { get; }
}
