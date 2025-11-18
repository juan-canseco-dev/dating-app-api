using DatingApp.Domain.Abstractions;

namespace DatingApp.Domain.Interests;

public class Interest : Entity<int>
{
    public int GroupId { get; }
    public string? Name { get; private set; }
    public virtual InterestGroup? Group { get; }
}
