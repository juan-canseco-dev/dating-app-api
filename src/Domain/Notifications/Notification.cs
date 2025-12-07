

using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Notifications;

public class Notification : Entity<Guid>
{
    public string FromUserId { get; }
    public string ToUserId { get; }
    public string Content { get; }
    public NotificationType Type { get; }
    public NotificationStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }

    private Notification()
    {
        FromUserId = default!;
        ToUserId = default!;
        Content = default!;
    }

    private Notification(string fromUserId, string toUserId, string content, NotificationType type, DateTime createdAt)
    {
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Content = content;
        Type = type;
        Status = NotificationStatus.Unseen;
        CreatedAt = createdAt;
    }

    public static Result<Notification> Create(string fromUserId, string toUserId, string content, NotificationType type, DateTime createdAt)
    {
        var notification = new Notification(fromUserId, toUserId, content, type, createdAt);
        return Result.Success(notification);
    }

    public Result MarkAsSeen(DateTime updatedAt)
    {
        if (Status == NotificationStatus.Seen)
        {
            return Result.Failure(NotificationErrors.NotificationAlreadySeen);
        }

        Status = NotificationStatus.Seen;
        UpdatedAt = updatedAt;
        return Result.Success();
    }
} 
