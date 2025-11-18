using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Orientations;

public sealed class Gender : Enumeration<Gender>
{
    public string Description { get; }
    private Gender(int id, string name, string description) : base(id, name)
    {
        Description = description;
    }


    public static readonly Gender Man =
        new(1, nameof(Man), "You identify as a man.");

    public static readonly Gender Woman =
        new(2, nameof(Woman), "You identify as a woman.");

    public static readonly Gender TransMan =
        new(3, "Trans Man", "You were assigned female at birth and identify as a man.");

    public static readonly Gender TransWoman =
        new(4, "Trans Woman", "You were assigned male at birth and identify as a woman.");

    public static readonly Gender NonBinary =
        new(5, "Non-Binary", "You don’t fit strictly into “man” or “woman.”");

    public static readonly Gender Genderqueer =
        new(6, "Queer", "Your gender is outside the usual binary.");

    public static readonly Gender Genderfluid =
        new(7, "Fliud", "Your gender can change or shift over time.");

}
