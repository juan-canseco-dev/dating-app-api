
using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Interests;

public static class InterestErrors
{

    public static Error NotFound(int interestId)
    {
        return new Error(
            Code: "Interest.NotFound",
            Description: $"The specified interest with the Id: {interestId} Was Not Found.",
            ErrorType: Error.Type.NotFound
        );
    }
}
