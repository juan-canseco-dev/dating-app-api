using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Swipes.Events;

public record UserPassedDomainEvent(
    string SwipeId,
    string FromUserId,
    string ToUserId
) : IDomainEvent;