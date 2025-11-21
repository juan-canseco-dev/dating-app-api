
using DatingApp.Domain.Abstractions;
using DatingApp.Domain.Matches.Events;
using DatingApp.Domain.Users;

namespace DatingApp.Domain.Matches;

public class Match : Entity<string>
{
    private Match()
    {
        PartnerOneId = default!;
        PartnerTwoId = default!;
    }

    private Match(string partnerOneId, string partnerTwoId, DateTime createdAt)
    {
        PartnerOneId = partnerOneId;
        PartnerTwoId = partnerTwoId;
        CreatedAt = createdAt;

        RaiseDomainEvent(new MatchCreatedDomainEvent(
            MatchId: Id!,
            PartnerOneId: partnerOneId,
            PartnerTwoId: partnerTwoId
        ));
    }

    public string PartnerOneId { get; }
    public string PartnerTwoId { get; }
    public DateTime CreatedAt { get; }

    public virtual User? PartnerOne { get; }
    public virtual User? PartnerTwo { get; }

    public static Result<Match> CreateNew(string partnerOneId, string partnerTwoId, DateTime createdAt)
    {

        if (partnerOneId == partnerTwoId)
            return Result.Failure<Match>(MatchErrors.CannotMatchSelf);

        var match = new Match(
            partnerOneId,
            partnerTwoId,
            createdAt
        );

        return Result.Success(match);
    }
}
