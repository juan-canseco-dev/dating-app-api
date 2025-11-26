using DatingApp.Domain.Abstractions;
using DatingApp.Domain.Chats.Events;
using DatingApp.Domain.Users;

namespace DatingApp.Domain.Chats;

public class Chat : Entity<Guid>
{
    private readonly List<Message> _messages = new();

    public string UserOneId { get; private set; }
    public string UserTwoId { get; private set; }
    public virtual User? UserOne { get; }
    public virtual User? UserTwo { get; } 


    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();

    private Chat() 
    {
        UserOneId = default!;
        UserTwoId = default!;
    }

    private Chat(string userOneId, string userTwoId)
    {
        UserOneId = userOneId;
        UserTwoId = userTwoId;
    }

    public Result<Message> AddMessage(string senderId, string receiverId, string content, DateTime sentAt)
    {
        if (senderId != UserOneId && senderId != UserTwoId)
        {
            return Result.Failure<Message>(ChatErrors.SenderNotInChat);
        }

        var message = new Message(senderId, receiverId, content, sentAt);
        _messages.Add(message);
        RaiseDomainEvent(new MessageSentEvent(Id, message.Id, senderId, receiverId, content, sentAt));
        return Result.Success(message);
    }

    public static Result<Chat> CreateNew(string userOneId, string userTwoId)
    {
        var chat = new Chat(userOneId, userTwoId);
        if (userOneId == userTwoId)
        {
            return Result.Failure<Chat>(ChatErrors.CannotChatSelf);
        }
        return Result.Success(chat);
    }
}
