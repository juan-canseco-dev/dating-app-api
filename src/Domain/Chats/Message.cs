namespace DatingApp.Domain.Chats;

public class Message
{
    public Guid Id { get; private set;  }
    public string SenderId { get; private set; }
    public string ReceiverId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentAt { get; private set; }

    private Message()
    {
        SenderId = default!;
        ReceiverId = default!;
        Content = default!;
    }

    public Message(string senderId, string receiverId, string content, DateTime sentAt)
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Content = content;
        SentAt = sentAt;
    }
}
