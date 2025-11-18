
using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Interests;

public class GroupErrors
{
    public static Error NotFound(int groupId)
    {
        return new Error(
            Code: "Group.NotFound",
            Description: $"The specified Group with the Id: {groupId} Was Not Found.",
            ErrorType: Error.Type.NotFound
        );
    }
}
