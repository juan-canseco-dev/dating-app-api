using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(string UserId) : IDomainEvent;
