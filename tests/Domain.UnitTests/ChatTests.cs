using DatingApp.Domain.Chats;
using DatingApp.Domain.Chats.Events;


namespace DatingApp.Domain.UnitTests;

public class ChatTests
{
    [Fact]
    public void CreateNew_ShouldReturnSuccess_WhenUsersAreDifferent()
    {
        // Arrange
        string userOne = "u1";
        string userTwo = "u2";

        // Act
        var result = Chat.CreateNew(userOne, userTwo);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userOne, result.Value.UserOneId);
        Assert.Equal(userTwo, result.Value.UserTwoId);
    }

    [Fact]
    public void CreateNew_ShouldFail_WhenUsersAreTheSame()
    {
        // Arrange
        string userId = "u1";

        // Act
        var result = Chat.CreateNew(userId, userId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ChatErrors.CannotChatSelf, result.Error);
    }

    // ---------------------------
    // AddMessage
    // ---------------------------

    [Fact]
    public void AddMessage_ShouldAddMessage_WhenSenderBelongsToChat()
    {
        // Arrange
        var chatResult = Chat.CreateNew("u1", "u2");
        var chat = chatResult.Value;

        string sender = "u1";
        string receiver = "u2";
        string content = "Hello!";
        DateTime sentAt = DateTime.UtcNow;

        // Act
        var result = chat.AddMessage(sender, receiver, content, sentAt);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(chat.Messages);
        Assert.Equal(content, chat.Messages[0].Content);
    }

    [Fact]
    public void AddMessage_ShouldFail_WhenSenderNotInChat()
    {
        // Arrange
        var chatResult = Chat.CreateNew("u1", "u2");
        var chat = chatResult.Value;

        // Act
        var result = chat.AddMessage("uX", "u1", "Hack!", DateTime.UtcNow);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ChatErrors.SenderNotInChat, result.Error);
        Assert.Empty(chat.Messages);
    }

    // ---------------------------
    // Domain Events
    // ---------------------------

    [Fact]
    public void AddMessage_ShouldRaiseMessageSentEvent()
    {
        // Arrange
        var chatResult = Chat.CreateNew("u1", "u2");
        var chat = chatResult.Value;

        string sender = "u1";
        string receiver = "u2";
        string content = "Test message";
        DateTime sentAt = DateTime.UtcNow;

        // Act
        chat.AddMessage(sender, receiver, content, sentAt);

        // Assert
        var evt = chat.GetDomainEvents().OfType<MessageSentEvent>().SingleOrDefault();

        Assert.NotNull(evt);
        Assert.Equal(chat.Id, evt.ChatId);
        Assert.Equal(sender, evt.SenderId);
        Assert.Equal(receiver, evt.ReceiverId);
        Assert.Equal(content, evt.Content);
    }

    [Fact]
    public void AddMessage_ShouldNotRaiseEvent_WhenSenderInvalid()
    {
        // Arrange
        var chatResult = Chat.CreateNew("u1", "u2");
        var chat = chatResult.Value;

        // Act
        chat.AddMessage("unknownUser", "u1", "Nope", DateTime.UtcNow);

        // Assert
        Assert.Empty(chat.GetDomainEvents());
    }
}

