
namespace DatingApp.Domain.Users;

public enum FlagStatus
{
    Active,              // Counts against the user
    Invalid,             // Moderator says the flag is wrong
    RemovedByModerator   // Flag was valid but moderator chose to clear it
}
