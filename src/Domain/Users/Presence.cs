namespace DatingApp.Domain.Users;

public class Presence
{
    public PresenceStatus Status { get; }
    public DateTime LastActivity { get; }
    public Presence(PresenceStatus status, DateTime lastActivity)
    {
        Status = status;
        LastActivity = lastActivity;
    }
}
