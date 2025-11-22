using DatingApp.Domain.Abstractions;
using DatingApp.Domain.Swipes.Events;
using DatingApp.Domain.Users;

namespace DatingApp.Domain.Swipes;

public class Swipe : Entity<string> {

    private Swipe() 
    {
        FromUserId = default!;
        ToUserId = default!;
    }

    private Swipe(string fromUserId, string toUserId, SwipeDirection direction, DateTime createdAt)
    {
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Direction = direction;
        CreatedAt = createdAt;

        var @event = GetDomainEventFromSwipe(this);
        RaiseDomainEvent(@event);
    }

    public string FromUserId {get;}
    public string ToUserId {get;}
    public SwipeDirection Direction {get;}
    public DateTime CreatedAt {get;}

    public virtual User? FromUser {get;}
    public virtual User? ToUser {get;}

   
    private IDomainEvent GetDomainEventFromSwipe(Swipe swipe)
    {
        return swipe.Direction switch
        {
            SwipeDirection.Like => new UserLikedDomainEvent(
                SwipeId: swipe.Id!,
                FromUserId: swipe.FromUserId,
                ToUserId: swipe.ToUserId
            ),

            SwipeDirection.Pass => new UserPassedDomainEvent(
                SwipeId: swipe.Id!,
                FromUserId: swipe.FromUserId,
                ToUserId: swipe.ToUserId
            ),

            _ => throw new InvalidOperationException($"SwipeDirection '{swipe.Direction}' has no domain event.")
        };
    }


    public static Result<Swipe> CreateNew(
        string fromUserId, 
        string toUserId, 
        SwipeDirection direction, 
        DateTime createdAt
    ) 
    {
        if (fromUserId == toUserId) 
        {
            return Result.Failure<Swipe>(SwipeErrors.CannotSwipeSelf);
        }

        if (!Enum.IsDefined(typeof(SwipeDirection), direction))
        {
            return Result.Failure<Swipe>(SwipeErrors.InvalidSwipeDirection);
        }

        var swipe = new Swipe(
            fromUserId,
            toUserId,
            direction,
            DateTime.UtcNow
        );

        return Result.Success(swipe);
    }
}