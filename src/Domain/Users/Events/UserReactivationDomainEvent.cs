

using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserReactivationDomainEvent(string UserId) : IDomainEvent;
