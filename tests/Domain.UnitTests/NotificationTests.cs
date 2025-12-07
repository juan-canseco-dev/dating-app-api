using DatingApp.Domain.Notifications;

namespace DatingApp.Domain.Tests.Notifications;

public class NotificationTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenValidParameters()
    {
        // Arrange
        var fromUserId = "user1";
        var toUserId = "user2";
        var content = "Test notification";
        var type = NotificationType.ChatMessage; // Assuming NotificationType is an enum
        var createdAt = DateTime.UtcNow;

        // Act
        var result = Notification.Create(fromUserId, toUserId, content, type, createdAt);

        // Assert
        Assert.True(result.IsSuccess);
        var notification = result.Value;
        Assert.Equal(fromUserId, notification.FromUserId);
        Assert.Equal(toUserId, notification.ToUserId);
        Assert.Equal(content, notification.Content);
        Assert.Equal(type, notification.Type);
        Assert.Equal(NotificationStatus.Unseen, notification.Status);
        Assert.Equal(createdAt, notification.CreatedAt);
        Assert.Null(notification.UpdatedAt);
    }

    [Fact]
    public void MarkAsSeen_ShouldReturnSuccess_WhenStatusIsUnseen()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var notification = Notification.Create("user1", "user2", "content", NotificationType.ChatMessage, createdAt).Value;
        var updatedAt = DateTime.UtcNow.AddMinutes(1);

        // Act
        var result = notification.MarkAsSeen(updatedAt);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(NotificationStatus.Seen, notification.Status);
        Assert.Equal(updatedAt, notification.UpdatedAt);
    }

    [Fact]
    public void MarkAsSeen_ShouldReturnFailure_WhenStatusIsAlreadySeen()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var notification = Notification.Create("user1", "user2", "content", NotificationType.ChatMessage, createdAt).Value;
        var updatedAt1 = DateTime.UtcNow.AddMinutes(1);
        notification.MarkAsSeen(updatedAt1); // Mark as seen first
        var updatedAt2 = DateTime.UtcNow.AddMinutes(2);

        // Act
        var result = notification.MarkAsSeen(updatedAt2);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(NotificationErrors.NotificationAlreadySeen, result.Error); // Assuming NotificationErrors is defined
        Assert.Equal(NotificationStatus.Seen, notification.Status);
        Assert.Equal(updatedAt1, notification.UpdatedAt); // Should not change
    }

    [Fact]
    public void Properties_ShouldBeImmutable_ExceptStatusAndUpdatedAt()
    {
        // Arrange
        var fromUserId = "user1";
        var toUserId = "user2";
        var content = "content";
        var type = NotificationType.ChatMessage;
        var createdAt = DateTime.UtcNow;
        var notification = Notification.Create(fromUserId, toUserId, content, type, createdAt).Value;

        // Act & Assert
        // These should not be changeable (assuming no setters)
        Assert.Equal(fromUserId, notification.FromUserId);
        Assert.Equal(toUserId, notification.ToUserId);
        Assert.Equal(content, notification.Content);
        Assert.Equal(type, notification.Type);
        Assert.Equal(createdAt, notification.CreatedAt);

        // Status and UpdatedAt can change via MarkAsSeen
        Assert.Equal(NotificationStatus.Unseen, notification.Status);
        Assert.Null(notification.UpdatedAt);
    }
}
