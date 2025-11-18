
using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Orientations;

public sealed class Attraction : Enumeration<Attraction>
{
    public static readonly Attraction Men = new(1, "Men", "Interested in men");
    public static readonly Attraction Women = new(2, "Women", "Interested in women");
    public static readonly Attraction BeyondGenderBinary = new(3, "Beyond the Gender Binary", "Interested in people beyond the gender binary");

    private Attraction(int id, string name, string description)
     : base(id, name)
    {
        Description = description;
    }
    public string Description { get; }

}
