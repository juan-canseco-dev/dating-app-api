using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Swipes;

public class SwipeErrors 
{
    public static readonly Error CannotSwipeSelf => new Error(
        Code: "Swipe.CannotSwipeSelf",
        Description: "A user cannot swipe themselves.",
        ErrorType: Error.Type.Domain
    );
    
    public static readonly Error InvalidSwipeDirection => new Error(
        Code: "Swipe.InvalidDirection",
        Description: "The provided swipe direction is invalid.",
        ErrorType: Error.Type.Domain
    );
}