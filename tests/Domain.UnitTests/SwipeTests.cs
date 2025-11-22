using DatingApp.Domain.Swipes;
using DatingApp.Domain.Swipes.Events;

namespace DatingApp.Domain.UnitTests;

public class SwipeTests
{
    private string ToUserId = "toUserId";
    private string FromUserId = "fromUserId";

    [Fact]
    public void CreateNew_ShouldRaiseCorrectDomainEvent_OnPass()
    {
        // Act
        var result = Swipe.CreateNew(FromUserId, ToUserId, SwipeDirection.Pass, DateTime.UtcNow);

        // Assert
        Assert.True(result.IsSuccess);
        var swipe = result.Value;
        var domainEvent = swipe.GetDomainEvents().FirstOrDefault();

        Assert.IsType<UserPassedDomainEvent>(domainEvent); // per your mapping
    }

    [Fact]
    public void CreateNew_ShouldRaiseCorrectDomainEvent_OnLike()
    {
        // Act
        var result = Swipe.CreateNew(FromUserId, ToUserId, SwipeDirection.Like, DateTime.UtcNow);

        // Assert
        Assert.True(result.IsSuccess);
        var swipe = result.Value;
        var domainEvent = swipe.GetDomainEvents().FirstOrDefault();

        Assert.IsType<UserLikedDomainEvent>(domainEvent);
    }


    [Fact]
    public void CreateNew_ShouldFail_WhenEnumValueIsNotDefined()
    {
        // Arrange
        var invalidDirection = (SwipeDirection)(-999);

        // Act
        var result = Swipe.CreateNew(
            FromUserId,
            ToUserId,
            invalidDirection,
            DateTime.UtcNow
        );

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(SwipeErrors.InvalidSwipeDirection, result.Error);
    }

}
