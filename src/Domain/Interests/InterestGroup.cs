using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Interests;

public class InterestGroup : Entity<int>
{
    public string? Name { get; set; }
    public virtual ICollection<Interest>? Interests { get; set; }
    private InterestGroup() { }
}
