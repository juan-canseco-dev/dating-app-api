

using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserProfileChangedDomainEvent(string UserId) : IDomainEvent;
