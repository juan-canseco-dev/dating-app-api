using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserUnblockedDomainEvent(string UserId, string UnblockedUserId) : IDomainEvent
{
    public DomainEventDeliveryMode DeliveryMode => DomainEventDeliveryMode.Delayed;
}