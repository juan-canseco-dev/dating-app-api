using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserWentOfflineDomainEvent(string UserId, DateTime LastActivity) : IDomainEvent
{
    public DomainEventDeliveryMode DeliveryMode => DomainEventDeliveryMode.Immediate;
}