using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Users;

public static class UserErrors
{
    public static Error InvalidAttractionsCount(int count)
    {
        return new Error(
            Code: "User.Attractions.InvalidCount",
            Description: $"The number of selected attractions ({count}) must be between 1 and 3.",
            ErrorType: Error.Type.Domain
        );
    }

    public static Error InvalidInterestsCount(int count)
    {
        return new Error(
            Code: "User.Interests.InvalidCount",
            Description: $"The number of selected interests ({count}) must be between 1 and 10.",
            ErrorType: Error.Type.Domain
        );
    }


    public static Error InvalidOrientationsCount(int count)
    {
        return new Error(
            Code: "User.Orientations.InvalidCount",
            Description: $"The number of selected orientations ({count}) must be between 1 and 3.",
            ErrorType: Error.Type.Domain
        );
    }

    public static Error InvalidPhotoUrlsCount(int count)
    {
        return new Error(
            Code: "User.Photos.InvalidCount",
            Description: $"The number of uploaded photos ({count}) must be between 1 and 9.",
            ErrorType: Error.Type.Domain
        );
    }

    public static Error EmailAlreadyExists(string email)
    {
        return new Error(
            Code: "User.Email.AlreadyExists",
            Description: $"The email '{email}' is already registered.",
            ErrorType: Error.Type.Domain
        );
    }

    public static Error AlreadySuspended(string userId)
    {
        return new Error(
            Code: "User.AlreadySuspended",
            Description: $"User '{userId}' is already suspended and cannot receive additional flags.",
            ErrorType: Error.Type.Domain
        );
    }

    public static Error FlagNotFound(string flagId)
    {
        return new Error(
            Code: "User.Flags.NotFound",
            Description: $"THe User Flag '{flagId}' was not Found.",
            ErrorType: Error.Type.NotFound
        );
    }

    public static Error FlagAlreadyReviewed(string flagId)
    {
        return new Error(
            Code: "User.Flags.AlreadyReviewed",
            Description: $"User Flag '{flagId}' is already reviewed.",
            ErrorType: Error.Type.Domain
        );
    }

    public static Error CannotBlockSelf(string userId) =>
       new("User.CannotBlockSelf", "You cannot block yourself.", Error.Type.Domain);

    public static Error AlreadyBlocked(string userId) =>
        new("User.AlreadyBlocked", $"User {userId} is already blocked.", Error.Type.Domain);

    public static Error NotBlocked(string userId) =>
        new("User.NotBlocked", $"User {userId} is not blocked.", Error.Type.Domain);

}
