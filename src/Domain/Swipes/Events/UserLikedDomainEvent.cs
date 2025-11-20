using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Swipes.Events;

public record UserLikedDomainEvent(
    string SwipeId,
    string FromUserId,
    string ToUserId
) : IDomainEvent;