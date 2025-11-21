
using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Matches.Events;

public sealed record MatchCreatedDomainEvent(
    string MatchId,
    string PartnerOneId,
    string PartnerTwoId
) : IDomainEvent;