using DatingApp.Domain.Abstractions;
using MediatR;

namespace DatingApp.Domain.Users.Events;

public sealed record UserSuspendedDomainEvent(string UserId) : IDomainEvent;
