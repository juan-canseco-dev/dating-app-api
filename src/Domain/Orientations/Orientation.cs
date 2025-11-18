using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Orientations;
public sealed class Orientation : Enumeration<Orientation>
{
    public string Description { get; } 

    private Orientation(int id, string name, string description) : base(id, name)
    {
        Description = description;
    }

    public static readonly Orientation Heterosexual =
       new(1, nameof(Heterosexual), "Into people of the opposite sex.");

    public static readonly Orientation Gay =
        new(2, nameof(Gay), "Into people of the same sex. Usually used by men who like men.");

    public static readonly Orientation Lesbian =
        new(3, nameof(Lesbian), "A woman who’s into other women.");

    public static readonly Orientation Bisexual =
        new(4, nameof(Bisexual), "Into more than one gender.");

    public static readonly Orientation Asexual =
        new(5, nameof(Asexual), "Doesn’t really feel sexual attraction toward others.");

    public static readonly Orientation Demisexual =
        new(6, nameof(Demisexual), "Only feels attraction after a real emotional connection.");

    public static readonly Orientation Pansexual =
        new(7, nameof(Pansexual), "Attracted to people regardless of gender or sex.");

    public static readonly Orientation Queer =
        new(8, nameof(Queer), "A broad and inclusive term for anyone who doesn’t fit the usual labels.");

    public static readonly Orientation Exploring =
        new(9, nameof(Exploring), "Still figuring things out — open to discovering what feels right.");
}
