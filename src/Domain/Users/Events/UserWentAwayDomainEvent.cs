using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserWentAwayDomainEvent(string UserId, DateTime LastActivity) : IDomainEvent
{
    public DomainEventDeliveryMode DeliveryMode => DomainEventDeliveryMode.Immediate;
}