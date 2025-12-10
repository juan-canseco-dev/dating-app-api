namespace DatingApp.Domain.Users;

public class Presence
{
    public string UserId { get; }
    public PresenceStatus Status { get; }
    public DateTime LastActivity { get; }
    public virtual User? User { get; } = default!;
    
    public Presence(PresenceStatus status, DateTime lastActivity)
    {
        UserId = default!;
        Status = status;
        LastActivity = lastActivity;
    }
    
    private Presence()
    {
        UserId = default!;
    }
}
