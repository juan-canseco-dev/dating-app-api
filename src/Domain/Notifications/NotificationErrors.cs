using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Notifications;

public static class NotificationErrors
{
    public static readonly Error NotificationAlreadySeen = new(
       Code: "Notification.AlreadySeen",
       Description: "The notification has already been marked as seen.",
       ErrorType: Error.Type.Domain
   );
}
