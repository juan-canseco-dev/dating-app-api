

using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserBlockedDomainEvent(string UserId, string BlockedUserId) : IDomainEvent;