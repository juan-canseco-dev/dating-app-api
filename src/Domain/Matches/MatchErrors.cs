
using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Matches;

public static class MatchErrors
{

    public static readonly Error CannotMatchSelf = new(
       Code: "Match.CannotMatchSelf",
       Description: "A partner cannot match with themselves.",
       ErrorType: Error.Type.Domain
   );

   
}
