

using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Chats.Events;

public sealed record MessageSentEvent(Guid ChatId, Guid MessageId, string SenderId, string ReceiverId, string Content, DateTime SentAt) : IDomainEvent
{
    public DomainEventDeliveryMode DeliveryMode => DomainEventDeliveryMode.Immediate;
}