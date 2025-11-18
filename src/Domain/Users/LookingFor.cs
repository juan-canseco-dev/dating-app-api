using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users;

public sealed class LookingFor : Enumeration<LookingFor>
{
    public static readonly LookingFor SeriousRelationship =
       new(1, "SeriousRelationship", "Looking for a meaningful, long-term relationship.");

    public static readonly LookingFor OpenToRelationship =
        new(2, "OpenToRelationship", "Interested in a relationship, but keeping an open mind.");

    public static readonly LookingFor FunButOpen =
        new(3, "FunButOpen", "Looking for fun, but open to something more.");

    public static readonly LookingFor ShortTermFun =
        new(4, "ShortTermFun", "Interested in short-term fun and casual connections.");

    public static readonly LookingFor MakeFriends =
        new(5, "MakeFriends", "Here to make new friends and meet people.");

    public static readonly LookingFor StillThinking =
        new(6, "StillThinking", "Still figuring out what I’m looking for.");


    private LookingFor(int id, string name, string description)
      : base(id, name)
    {
        Description = description;
    }

    public string Description { get; }
}
