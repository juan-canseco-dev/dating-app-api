using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users.Events;

public sealed record UserConfirmedDomainEvent(string UserId) : IDomainEvent;
