using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserWentOnlineDomainEvent(string UserId, DateTime LastActivity) : IDomainEvent
{
    DomainEventDeliveryMode IDomainEvent.DeliveryMode => DomainEventDeliveryMode.Immediate;
}
