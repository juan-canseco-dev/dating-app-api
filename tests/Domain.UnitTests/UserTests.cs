using DatingApp.Domain.Users;
using DatingApp.Domain.Orientations;
using DatingApp.Domain.Interests;
using DatingApp.Domain.Users.Events;

namespace DatingApp.Domain.UnitTests;

public class UserTests
{
    private readonly HashSet<Attraction> validAttractions =
        new() { Attraction.Men };

    private readonly HashSet<Orientation> validOrientations =
        new() { Orientation.Bisexual };

    private readonly HashSet<Interest> validInterests =
        new() { new Interest() };

    private readonly HashSet<string> validPhotos =
        new() { "photo1.jpg" };

    private readonly Gender gender = Gender.Man;
    private readonly LookingFor lookingFor = LookingFor.MakeFriends;

    private const string ValidId = "user-001";
    private const string Email = "test@test.com";
    private const string DisplayName = "John";
    private const string Description = "Hello world";


    // -----------------------------
    // CREATE NEW USER
    // -----------------------------
    [Fact]
    public void CreateNew_Should_ReturnSuccess_When_Valid()
    {
        var result = User.CreateNew(
            ValidId,
            Email,
            DisplayName,
            Description,
            gender,
            lookingFor,
            validAttractions,
            validOrientations,
            validInterests,
            validPhotos
        );

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Single(result.Value.GetDomainEvents());
        Assert.IsType<UserCreatedDomainEvent>(result.Value.GetDomainEvents().FirstOrDefault());
    }


    [Fact]
    public void CreateNew_Should_Fail_When_AttractionsInvalid()
    {
        var invalid = new HashSet<Attraction>();

        var result = User.CreateNew(
            ValidId,
            Email,
            DisplayName,
            Description,
            gender,
            lookingFor,
            invalid,
            validOrientations,
            validInterests,
            validPhotos
        );

        Assert.True(result.IsFailure);
        Assert.Equal("User.Attractions.InvalidCount", result.Error.Code);
    }


    [Fact]
    public void CreateNew_Should_Fail_When_InterestsInvalid()
    {
        var invalid = new HashSet<Interest>();

        var result = User.CreateNew(
            ValidId,
            Email,
            DisplayName,
            Description,
            gender,
            lookingFor,
            validAttractions,
            validOrientations,
            invalid,
            validPhotos
        );

        Assert.True(result.IsFailure);
        Assert.Equal("User.Interests.InvalidCount", result.Error.Code);
    }


    [Fact]
    public void CreateNew_Should_Fail_When_OrientationsInvalid()
    {
        var invalid = new HashSet<Orientation>();

        var result = User.CreateNew(
            ValidId,
            Email,
            DisplayName,
            Description,
            gender,
            lookingFor,
            validAttractions,
            invalid,
            validInterests,
            validPhotos
        );

        Assert.True(result.IsFailure);
        Assert.Equal("User.Orientations.InvalidCount", result.Error.Code);
    }


    [Fact]
    public void CreateNew_Should_Fail_When_PhotoUrlsInvalid()
    {
        var invalid = new HashSet<string>();

        var result = User.CreateNew(
            ValidId,
            Email,
            DisplayName,
            Description,
            gender,
            lookingFor,
            validAttractions,
            validOrientations,
            validInterests,
            invalid
        );

        Assert.True(result.IsFailure);
        Assert.Equal("User.Photos.InvalidCount", result.Error.Code);
    }


    // -----------------------------
    // UPDATE PROFILE
    // -----------------------------
    private User CreateValidUser()
    {
        return User.CreateNew(
            ValidId,
            Email,
            DisplayName,
            Description,
            gender,
            lookingFor,
            validAttractions,
            validOrientations,
            validInterests,
            validPhotos
        ).Value!;
    }

    [Fact]
    public void UpdateProfile_Should_Succeed()
    {
        var user = CreateValidUser();

        var result = user.UpdateProfile(
            "NewName",
            "NewDesc",
            lookingFor,
            validAttractions,
            validInterests,
            validPhotos
        );

        Assert.True(result.IsSuccess);
        Assert.Equal("NewName", user.DisplayName);
        Assert.IsType<UserProfileChangedDomainEvent>(user.GetDomainEvents().LastOrDefault());
    }


    [Fact]
    public void UpdateProfile_Should_Fail_When_AttractionsInvalid()
    {
        var user = CreateValidUser();

        var result = user.UpdateProfile(
            "A",
            "B",
            lookingFor,
            new HashSet<Attraction>(),
            validInterests,
            validPhotos
        );

        Assert.True(result.IsFailure);
    }


    // -----------------------------
    // ADD FLAG
    // -----------------------------
    [Fact]
    public void AddFlag_Should_Add_And_NotSuspend()
    {
        var user = CreateValidUser();

        var result = user.AddFlag("admin", "reason", DateTime.UtcNow, FlagReason.Spam);

        Assert.True(result.IsSuccess);
        Assert.Single(user.Flags);
        Assert.False(user.Suspended);
    }


    [Fact]
    public void AddFlag_Should_Suspend_After_ThreeFlags()
    {
        var user = CreateValidUser();

        user.AddFlag("admin", "1", DateTime.UtcNow, FlagReason.Spam);
        user.AddFlag("admin", "2", DateTime.UtcNow, FlagReason.Harassment);
        var result = user.AddFlag("admin", "3", DateTime.UtcNow, FlagReason.FakeProfile);

        Assert.True(user.Suspended);
        Assert.True(result.IsSuccess);

        Assert.Contains(user.GetDomainEvents(), e => e is UserSuspendedDomainEvent);
    }


    [Fact]
    public void AddFlag_Should_Fail_When_UserSuspended()
    {
        var user = CreateValidUser();

        user.AddFlag("1", "a", DateTime.UtcNow, FlagReason.Spam);
        user.AddFlag("2", "b", DateTime.UtcNow, FlagReason.Harassment);
        user.AddFlag("3", "c", DateTime.UtcNow, FlagReason.Other);

        var result = user.AddFlag("4", "d", DateTime.UtcNow, FlagReason.Spam);

        Assert.True(result.IsFailure);
        Assert.Equal("User.AlreadySuspended", result.Error.Code);
    }


    // -----------------------------
    // REVIEW FLAG
    // -----------------------------
    [Fact]
    public void ReviewFlag_Should_Fail_When_NotFound()
    {
        var user = CreateValidUser();
        var flagId = "not-exist";
        var expectedError = UserErrors.FlagNotFound(flagId);
        var result = user.ReviewFlag(flagId, "admin", "ok", true, DateTime.UtcNow);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }


    [Fact]
    public void ReviewFlag_Should_Fail_When_AlreadyReviewed()
    {
        var user = CreateValidUser();

        user.AddFlag("admin", "test", DateTime.UtcNow, FlagReason.Spam);
        var flag = user.Flags.First();
        var expectedError = UserErrors.FlagAlreadyReviewed(flag.Id);

        user.ReviewFlag(flag.Id, "admin2", "comment2", true, DateTime.UtcNow);
        var result =  user.ReviewFlag(flag.Id, "admin3", "Comment3", false, DateTime.UtcNow);

        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }


    [Fact]
    public void ReviewFlag_Should_Reactivate_User_When_Flags_Are_Below_Threshold()
    {
        var user = CreateValidUser();

        // suspend user
        user.AddFlag("1", "a", DateTime.UtcNow, FlagReason.Spam);
        user.AddFlag("2", "b", DateTime.UtcNow, FlagReason.Spam);
        user.AddFlag("3", "c", DateTime.UtcNow, FlagReason.Spam);

        var flag = user.Flags.First();

        // review as invalid
        var result = user.ReviewFlag(flag.Id, "admin", "ok", false, DateTime.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.False(user.Suspended);
        Assert.Contains(user.GetDomainEvents(), e => e is UserReactivationDomainEvent);
    }


    // -----------------------------
    // BLOCK / UNBLOCK
    // -----------------------------
    [Fact]
    public void BlockUser_Should_Fail_When_Self()
    {
        var user = CreateValidUser();

        var result = user.BlockUser(user.Id, DateTime.UtcNow);

        Assert.True(result.IsFailure);
    }


    [Fact]
    public void BlockUser_Should_Succeed()
    {
        var user = CreateValidUser();

        var result = user.BlockUser("x", DateTime.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Contains(user.GetDomainEvents(), e => e is UserBlockedDomainEvent);
    }


    [Fact]
    public void BlockUser_Should_Fail_When_AlreadyBlocked()
    {
        var user = CreateValidUser();

        user.BlockUser("x", DateTime.UtcNow);
        var result = user.BlockUser("x", DateTime.UtcNow);

        Assert.True(result.IsFailure);
    }


    [Fact]
    public void UnblockUser_Should_Succeed()
    {
        var user = CreateValidUser();

        user.BlockUser("target", DateTime.UtcNow);

        var result = user.UnblockUser("target");

        Assert.True(result.IsSuccess);
        Assert.IsType<UserUnblockedDomainEvent>(user.GetDomainEvents().Last());
    }


    [Fact]
    public void UnblockUser_Should_Fail_When_NotBlocked()
    {
        var user = CreateValidUser();

        var result = user.UnblockUser("nope");

        Assert.True(result.IsFailure);
    }
}
