using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Swipes;

public static class SwipeErrors 
{
    public static Error CannotSwipeSelf => new Error(
        Code: "Swipe.CannotSwipeSelf",
        Description: "A user cannot swipe themselves.",
        ErrorType: Error.Type.Domain
    );
    
    public static Error InvalidSwipeDirection => new Error(
        Code: "Swipe.InvalidDirection",
        Description: "The provided swipe direction is invalid.",
        ErrorType: Error.Type.Domain
    );
}