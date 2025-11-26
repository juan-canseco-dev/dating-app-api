

using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Chats;

public class ChatErrors
{
    public static Error CannotChatSelf = new Error(
        Code:"Chat.CannotChatSelf",
        Description: "Cannot create a chat with yourself.",
        Error.Type.Domain
    );

    public static Error SenderNotInChat = new Error(
        Code: "Chat.SenderNotInChat", 
        Description: "",
        Error.Type.Domain
    );
}
