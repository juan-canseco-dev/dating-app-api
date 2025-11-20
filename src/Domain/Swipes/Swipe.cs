using DatingApp.Domain.Abstractions;
using DatingApp.Domain.Users;

namespace DatingApp.Domain.Swipes;

public class Swipe : Entity<string> {

    private Swipe() {}

    private Swipe(string fromUserId, string toUserId, SwipeDirection direction, DateTime createdAt)
    {
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Direction = direction;
        CreatedAt = createdAt;
    }

    public string FromUserId {get;}
    public string ToUserId {get;}
    public SwipeDirection Direction {get;}
    public DateTime CreatedAt {get;}

    public virtual? User FromUser {get;}
    public virtual? User ToUser {get;}

   private static IDomainEvent ToDomainEvent(Swipe swipe)
    {
        return swipe.Direction switch
        {
            SwipeDirection.Right => new UserLikedDomainEvent(
                SwipeId: swipe.Id,
                FromUserId: swipe.FromUserId,
                ToUserId: swipe.ToUserId
            ),

            SwipeDirection.Left => new UserPassedDomainEvent(
                SwipeId: swipe.Id,
                FromUserId: swipe.FromUserId,
                ToUserId: swipe.ToUserId
            ),

            _ => throw new InvalidOperationException($"SwipeDirection '{swipe.Direction}' has no domain event.")
        };
    }


    public Result<Swipe> CreateNew(string fromUserId, string toUserId, SwipeDirection direction) 
    {
        if (fromUserId == toUserId) 
        {
            return Result.Fialure<Swipe>(SwipeErrors.CannotSwipeSelf);
        }

        if (!Enum.IsDefined(typeof(SwipeDirection), direction))
        {
            return Result.Failure<Swipe>(SwipeErrors.InvalidSwipeDirection);
        }

        var swipe = new Swipe(
            FromUserId: fromUserId,
            ToUserId: toUserId,
            Direction: direction,
            CreatedAt: DateTime.UtcNow
        );

        var @event = ToDomainEvent(swipe);
        
        RaiseDomainEvent(@event);

        return Result.Success(swipe);
    }
}