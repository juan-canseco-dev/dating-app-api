using DatingApp.Domain.Abstractions;
using DatingApp.Domain.Interests;
using DatingApp.Domain.Orientations;
using DatingApp.Domain.Users.Events;
using System.Linq;

namespace DatingApp.Domain.Users;

public class User : Entity<string>
{
    private readonly int maxNumberOfFlags = 3;
    private readonly List<Attraction> _attractions = new();
    private readonly List<Interest> _interests = new();
    private readonly List<Orientation> _orientations = new();
    private readonly List<string> _photoUrls = new();
    private readonly List<UserFlag> _flags = new();
    private readonly List<UserBlock> _blockedUsers = new();

    private User()
    {
        Id = default!;
        Email = default!;
        DisplayName = default!;
        PhotoUrl = default!;
        Description = default!;
    }

    private User(
        string id,
        string email,
        string displayName,
        string description,
        Gender gender,
        LookingFor lookingFor,
        List<Attraction> attractions,
        List<Orientation> orientations,
        List<Interest> interests,
        List<string> photoUrls
        )
    {

        Id = id;
        Email = email;
        DisplayName = displayName;
        Description = description;
        PhotoUrl = photoUrls[0];
        GenderId = gender.Id;
        LookingForId = lookingFor.Id;
        _attractions.AddRange(attractions);
        _orientations.AddRange(orientations);
        _interests.AddRange(interests);
        _photoUrls.AddRange(photoUrls);
        RaiseDomainEvent(new UserCreatedDomainEvent(Id));
    }

    public string Email { get; }
    public string DisplayName { get; private set; }
    public string PhotoUrl { get; private set; }
    public string Description { get; private set; }
    public int GenderId { get; }
    public int LookingForId { get; private set; }
    public IReadOnlyCollection<Interest> Interests => _interests.AsReadOnly();
    public IReadOnlyCollection<Orientation> Orientations => _orientations.AsReadOnly();
    public IReadOnlyCollection<string> PhotoUrls => _photoUrls.AsReadOnly();
    public IReadOnlyCollection<UserFlag> Flags => _flags.AsReadOnly();
    public IReadOnlyCollection<UserBlock> BlockedUsers => _blockedUsers.AsReadOnly();
    public virtual Gender? Gender { get; }

    public virtual LookingFor? LookingFor { get; }
    public DateTime CreatedAt { get; }
    public bool Confirmed { get; }
    public DateTime ConfirmedAt { get; }
    public DateTime UpdatedAt { get; }
    public bool Suspended { get; private set; } = false;

    public static Result<User> CreateNew(
        string id,
        string email,
        string displayName,
        string description,
        Gender gender,
        LookingFor lookingFor,
        HashSet<Attraction> attractions,
        HashSet<Orientation> orientations,
        HashSet<Interest> interests,
        HashSet<string> photoUrls
    )
    {

        if (attractions.Count < 1 && interests.Count > 3)
        {
            return Result.Failure<User>(
                UserErrors.InvalidAttractionsCount(attractions.Count)
            );
        }

        if (interests.Count < 1 && interests.Count > 10)
        {
            return Result.Failure<User>(
                UserErrors.InvalidInterestsCount(interests.Count)
             );
        }

        if (orientations.Count < 1 && interests.Count > 3)
        {
            return Result.Failure<User>(
               UserErrors.InvalidOrientationsCount(orientations.Count)
            );
        }

        if (photoUrls.Count < 1 && photoUrls.Count > 9)
        {
            return Result.Failure<User>(
               UserErrors.InvalidPhotoUrlsCount(photoUrls.Count)
            );
        }

        

        return Result.Success(new User(
            id,
            email,
            displayName,
            description,
            gender,
            lookingFor,
            attractions.ToList(),
            orientations.ToList(),
            interests.ToList(),
            photoUrls.ToList()
        ));
    }

    public Result UpdateProfile(
        string displayName, 
        string description,
        LookingFor lookingFor,
        HashSet<Attraction> attractions,
        HashSet<Interest> interests,
        HashSet<string> photoUrls)
    {


        if (attractions.Count < 1 && interests.Count > 3)
        {
            return Result.Failure<User>(
                UserErrors.InvalidAttractionsCount(attractions.Count)
            );
        }

        if (interests.Count < 1 && interests.Count > 10)
        {
            return Result.Failure<User>(
                UserErrors.InvalidInterestsCount(interests.Count)
             );
        }

        if (photoUrls.Count < 1 && photoUrls.Count > 9)
        {
            return Result.Failure<User>(
               UserErrors.InvalidPhotoUrlsCount(photoUrls.Count)
            );
        }

        DisplayName = displayName;
        Description = description;
        LookingForId = lookingFor.Id;

        _attractions.Clear();
        _attractions.AddRange( attractions );
        _interests.Clear();
        _interests.AddRange(interests);
        _photoUrls.Clear();
        _photoUrls.AddRange(photoUrls);

        RaiseDomainEvent(new UserProfileChangedDomainEvent(Id!));

        return Result.Success();
    }

    public Result AddFlag(string reportedBy, string comment, DateTime reportedAt, FlagReason reason)
    {
        if (Suspended)
        {
            return Result.Failure<User>(UserErrors.AlreadySuspended(Id!));
        }

        _flags.Add(new UserFlag(reportedBy, comment, reportedAt, reason));


        var activeFlagsCount = _flags.Count(x => x.Status == FlagStatus.Active);

        if (activeFlagsCount == maxNumberOfFlags)
        {
            Suspended = true;
            RaiseDomainEvent(new UserSuspendedDomainEvent(Id!));
        }

        return Result.Success();
    }

    public Result ReviewFlag(
        string flagId, 
        string reviewerId, 
        string reviewerComment, 
        bool isValid, 
        DateTime reviewedAt)
    {
        var flag = _flags.FirstOrDefault(x => x.Id == flagId);

        if (flag == null)
        {
            return Result.Failure<User>(UserErrors.FlagNotFound(flagId));  
        }

        if (flag.IsReviewed)
        {
            return Result.Failure<User>(UserErrors.FlagAlreadyReviewed(flagId));
        }

        var status = isValid ? FlagStatus.Active : FlagStatus.Invalid;
        flag.Review(reviewerId, reviewerComment, reviewedAt, status);


        if (Suspended)
        {

            var activeFlagsCount = _flags.Count(x => x.Status == FlagStatus.Active);
            if (activeFlagsCount < maxNumberOfFlags)
            {
                Suspended = false;
            }
        }

        RaiseDomainEvent(new UserReactivationDomainEvent(Id!));

        return Result.Success();
    }

    public Result BlockUser(string blockedUserId, DateTime blockedAt)
    {
        if (blockedUserId == Id)
        {
            return Result.Failure(UserErrors.CannotBlockSelf(Id!));
        }

        if (_blockedUsers.Any(m => m.BlockedUserId == blockedUserId))
        {
            return Result.Failure(UserErrors.AlreadyBlocked(blockedUserId));
        }

        _blockedUsers.Add(new UserBlock(blockedUserId, blockedAt));
        RaiseDomainEvent(new UserBlockedDomainEvent(Id!, blockedUserId));

        return Result.Success();
    }

    public Result UnblockUser(string blockedUserId)
    {

        var blockedUser = _blockedUsers.FirstOrDefault(m => m.BlockedUserId == blockedUserId);

        if (blockedUser == null)
        {
            return Result.Failure(UserErrors.NotBlocked(blockedUserId));
        }

        _blockedUsers.Remove(blockedUser);
        RaiseDomainEvent(new UserUnblockedDomainEvent(Id!, blockedUserId));

        return Result.Success();
    }


}